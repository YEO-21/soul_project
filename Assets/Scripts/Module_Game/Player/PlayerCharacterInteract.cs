using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 상호작용 기능을 나타내는 컴포넌트입니다.
/// </summary>
public sealed class PlayerCharacterInteract : MonoBehaviour
{
    [Header("# 상호작용 레이어")]
    public LayerMask m_PlayerInteractableLayer;

    /// <summary>
    /// 상호작용 가능한 객체들을 담기 위한 리스트
    /// </summary>
    private List<IPlayerInteractable> _Interactables = new();

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
        
    }

    private void Update()
    {
        CheckInteractableObject();
    }

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

        if(detectedCollisions.Length != 0)
        {
           
           
            foreach(Collider collision in detectedCollisions)

            {

                DrawGizmoLineInfo checkLine;

                Vector3 start = transform.position + Vector3. up;

                Vector3 checkDirection = (collision.transform.position - transform.forward).normalized;

                Ray ray = new Ray(start, checkDirection);
                RaycastHit hit;
                if(PhysicsExt.Raycast(out checkLine, ray, out hit, 3.0f, int.MaxValue, QueryTriggerInteraction.Collide))
                {
                    IPlayerInteractable interactable = hit.transform.gameObject.GetComponent<IPlayerInteractable>();

                    // 상호작용 가능한 객체를 찾은 경우
                    if (interactable != null)
                    {
                        _Interactables.Add(interactable);
                        Debug.Log(hit.transform.gameObject.name);
                    }
                }


                _DebugDrawInteractableLines.Add(checkLine);
            }

            if(_Interactables.Count > 1)
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

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        PhysicsExt.DrawGizmoSphere(_DebugDrawInteractableArea);

        foreach (DrawGizmoLineInfo info in _DebugDrawInteractableLines)
            PhysicsExt.DrawGizmoLine(info);

    }
#endif 


}
