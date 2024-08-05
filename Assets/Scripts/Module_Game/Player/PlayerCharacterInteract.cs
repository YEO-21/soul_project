using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상호작용 기능을 나타내는 컴포넌트입니다.
/// </summary>
public sealed class PlayerCharacterInteract : MonoBehaviour
{
    [Header("# 상호작용 레이어")]
    public LayerMask m_PlayerInteractableLayer;

    private GameScenePlayerController _PlayerController;

    /// <summary>
    /// 현재 사용중인 Npc 상호작용 UI 객체
    /// </summary>
    private NpcInteractUIPanel _CurrentNpcInteractUIPanel;

    /// <summary>
    /// 현재 상호작용중인 객체
    /// </summary>
    private IPlayerInteractable _CurrentInteractionTarget;

    /// <summary>
    /// 상호작용 가능한 객체들을 담기 위한 리스트
    /// </summary>
    private List<IPlayerInteractable> _Interactables = new();

    #region 이벤트
    /// <summary>
    /// 상호작용 시작 이벤트
    /// </summary>
    public event System.Action<IPlayerInteractable> onInteractStarted;

    /// <summary>
    /// 상호작용 끝 이벤트
    /// </summary>
    public event System.Action onInteractFinished;
    #endregion

    #region 디버그
    private DrawGizmoSphereInfo _DebugDrawInteractableArea;
    private List<DrawGizmoLineInfo> _DebugDrawInteractableLines = new();
    #endregion

    /// <summary>
    /// 컴포넌트 초기화
    /// </summary>
    /// <param name="playerCharacter"></param>
    public void Initialize(PlayerCharacter playerCharacter)
    {
        _PlayerController = playerCharacter.playerController as GameScenePlayerController;
    }

    private void Update()
       =>CheckInteractableObject();

    /// <summary>
    /// 상호작용 가능한 객체를 확인합니다.
    /// </summary>
    private void CheckInteractableObject()
    {
        Collider[] detectedCollisions = PhysicsExt.OverlapSphere(
            out _DebugDrawInteractableArea,
            transform.position,
            3.0f,
            m_PlayerInteractableLayer,
            QueryTriggerInteraction.Collide);

        _Interactables.Clear();
        _DebugDrawInteractableLines.Clear();

        if (detectedCollisions.Length != 0)
        {
            foreach (Collider collision in detectedCollisions)
            {
                DrawGizmoLineInfo checkLine;

                Vector3 start = transform.position + Vector3.up;

                // 감지한 객체로의 방향
                Vector3 checkDirection = 
                    (collision.transform.position - start).normalized;

                Ray ray = new Ray(start, checkDirection);
                RaycastHit hit;
                
                if (PhysicsExt.Raycast(
                    out checkLine,
                    ray,
                    out hit,
                    3.0f,
                    int.MaxValue,
                    QueryTriggerInteraction.Collide))
                {
                    IPlayerInteractable interactable =
                        hit.transform.gameObject.GetComponent<IPlayerInteractable>();

                    // 상호작용 가능한 객체를 찾은 경우
                    if (interactable != null)
                    {
                        _Interactables.Add(interactable);
                    }
                }

                _DebugDrawInteractableLines.Add(checkLine);
            }

            if (_Interactables.Count > 1)
            {
                _Interactables.Sort((item1, item2) =>
                {
                    float item1Dist = Vector3.Distance(transform.position, item1.transform.position);
                    float item2Dist = Vector3.Distance(transform.position, item2.transform.position);

                    return item1Dist < item2Dist ? -1 : 1;
                });
            }
        }
    }

    /// <summary>
    /// 상호작용 키 입력 시 호출됩니다.
    /// </summary>
    public void OnInteractInput()
    {
        // 상호작용 가능한 객체가 존재하지 않는다면 함수 호출 종료.
        if (_Interactables.Count == 0) return;

        // 상호작용 가능한 객체 하나를 얻습니다.
        _CurrentInteractionTarget = _Interactables[0];


        // 상호작용 UI 띄우기
        //GameSceneUIInstance uiInstance = SceneManagerBase.instance.sceneInstance.
        //    playerController.uiInstance as GameSceneUIInstance;
        GameSceneUIInstance uiInstance = _PlayerController.uiInstance as GameSceneUIInstance;

        _CurrentNpcInteractUIPanel = uiInstance.OpenNpcInteractUI(_CurrentInteractionTarget.npcInfo);

        // UI 닫힘 콜백 등록
        _CurrentNpcInteractUIPanel.onUIClosed += CALLBACK_OnInteractUIClosed;

        // Npc 객체의 상호작용 시작 메서드 호출
        _CurrentInteractionTarget.OnInteractStarted(_CurrentNpcInteractUIPanel);

        // 입력 모드를 UI모드로 설정합니다.
        _PlayerController.SetInputMode(Constants.INPUTMODE_UI, true);

        // 상호작용 이벤트 발생
        onInteractStarted?.Invoke(_CurrentInteractionTarget);
    }
    
    /// <summary>
    /// UI 닫기 입력
    /// </summary>
    public void EscapeUIMode()
    {
        // UI 가 띄어진 경우
        if(_CurrentNpcInteractUIPanel)
        {
            // UI 닫기
            _CurrentNpcInteractUIPanel.CloseUI();

        }
    }


    private void CALLBACK_OnInteractUIClosed()
    {

        // 상호작용 종료
        _CurrentInteractionTarget.OnInteractFinished();

        // 상호작용 대상 비우기
        _CurrentInteractionTarget = null;

        // 상호작용 UI 비우기
        _CurrentNpcInteractUIPanel = null;

        // 입력 모드를 GameMode로 설정합니다.
        _PlayerController.SetInputMode(Constants.INPUTMODE_GAME, false);

        // 상호작용 끝 이벤트 발생
        onInteractFinished?.Invoke();

    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        PhysicsExt.DrawOverlapSphere(_DebugDrawInteractableArea);

        foreach(DrawGizmoLineInfo info in _DebugDrawInteractableLines)
        {
            PhysicsExt.DrawGizmoLine(info);
        }
    }
#endif

}
