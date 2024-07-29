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
    /// ��ȣ�ۿ� ������ ��ü���� ��� ���� ����Ʈ
    /// </summary>
    private List<IPlayerInteractable> _Interactables = new();

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
    {
        CheckInteractableObject();
    }

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
        IPlayerInteractable firstInteractable = _Interactables[0];

        // Npc ��ü�� ��ȣ�ۿ� ���� �޼��� ȣ��
        firstInteractable.OnInteractStarted();

        // ��ȣ�ۿ� UI ����
        //GameSceneUIInstance uiInstance = SceneManagerBase.instance.sceneInstance.
        //    playerController.uiInstance as GameSceneUIInstance;
        GameSceneUIInstance uiInstance = _PlayerController.uiInstance as GameSceneUIInstance;
        uiInstance.OpenNpcInteractUI(firstInteractable.npcInfo);
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
