using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ȣ�ۿ� ����� ��Ÿ���� ������Ʈ�Դϴ�.
/// </summary>
public sealed class PlayerCharacterInteract : MonoBehaviour
{
    [Header("# ��ȣ�ۿ� ���̾�")]
    public LayerMask m_PlayerInteractableLayer;

    private GameScenePlayerController _PlayerController;

    /// <summary>
    /// ���� ������� Npc ��ȣ�ۿ� UI ��ü
    /// </summary>
    private NpcInteractUIPanel _CurrentNpcInteractUIPanel;

    /// <summary>
    /// ���� ��ȣ�ۿ����� ��ü
    /// </summary>
    private IPlayerInteractable _CurrentInteractionTarget;

    /// <summary>
    /// ��ȣ�ۿ� ������ ��ü���� ��� ���� ����Ʈ
    /// </summary>
    private List<IPlayerInteractable> _Interactables = new();

    #region �̺�Ʈ
    /// <summary>
    /// ��ȣ�ۿ� ���� �̺�Ʈ
    /// </summary>
    public event System.Action<IPlayerInteractable> onInteractStarted;

    /// <summary>
    /// ��ȣ�ۿ� �� �̺�Ʈ
    /// </summary>
    public event System.Action onInteractFinished;
    #endregion

    #region �����
    private DrawGizmoSphereInfo _DebugDrawInteractableArea;
    private List<DrawGizmoLineInfo> _DebugDrawInteractableLines = new();
    #endregion

    /// <summary>
    /// ������Ʈ �ʱ�ȭ
    /// </summary>
    /// <param name="playerCharacter"></param>
    public void Initialize(PlayerCharacter playerCharacter)
    {
        _PlayerController = playerCharacter.playerController as GameScenePlayerController;
    }

    private void Update()
       =>CheckInteractableObject();

    /// <summary>
    /// ��ȣ�ۿ� ������ ��ü�� Ȯ���մϴ�.
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

                // ������ ��ü���� ����
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

                    // ��ȣ�ۿ� ������ ��ü�� ã�� ���
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
    /// ��ȣ�ۿ� Ű �Է� �� ȣ��˴ϴ�.
    /// </summary>
    public void OnInteractInput()
    {
        // ��ȣ�ۿ� ������ ��ü�� �������� �ʴ´ٸ� �Լ� ȣ�� ����.
        if (_Interactables.Count == 0) return;

        // ��ȣ�ۿ� ������ ��ü �ϳ��� ����ϴ�.
        _CurrentInteractionTarget = _Interactables[0];


        // ��ȣ�ۿ� UI ����
        //GameSceneUIInstance uiInstance = SceneManagerBase.instance.sceneInstance.
        //    playerController.uiInstance as GameSceneUIInstance;
        GameSceneUIInstance uiInstance = _PlayerController.uiInstance as GameSceneUIInstance;

        _CurrentNpcInteractUIPanel = uiInstance.OpenNpcInteractUI(_CurrentInteractionTarget.npcInfo);

        // UI ���� �ݹ� ���
        _CurrentNpcInteractUIPanel.onUIClosed += CALLBACK_OnInteractUIClosed;

        // Npc ��ü�� ��ȣ�ۿ� ���� �޼��� ȣ��
        _CurrentInteractionTarget.OnInteractStarted(_CurrentNpcInteractUIPanel);

        // �Է� ��带 UI���� �����մϴ�.
        _PlayerController.SetInputMode(Constants.INPUTMODE_UI, true);

        // ��ȣ�ۿ� �̺�Ʈ �߻�
        onInteractStarted?.Invoke(_CurrentInteractionTarget);
    }
    
    /// <summary>
    /// UI �ݱ� �Է�
    /// </summary>
    public void EscapeUIMode()
    {
        // UI �� ����� ���
        if(_CurrentNpcInteractUIPanel)
        {
            // UI �ݱ�
            _CurrentNpcInteractUIPanel.CloseUI();

        }
    }


    private void CALLBACK_OnInteractUIClosed()
    {

        // ��ȣ�ۿ� ����
        _CurrentInteractionTarget.OnInteractFinished();

        // ��ȣ�ۿ� ��� ����
        _CurrentInteractionTarget = null;

        // ��ȣ�ۿ� UI ����
        _CurrentNpcInteractUIPanel = null;

        // �Է� ��带 GameMode�� �����մϴ�.
        _PlayerController.SetInputMode(Constants.INPUTMODE_GAME, false);

        // ��ȣ�ۿ� �� �̺�Ʈ �߻�
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
