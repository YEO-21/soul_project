

using UnityEngine;
/// <summary>
/// �� ĳ������ ���� ���� ������ �˻��ϱ� ���� �����Դϴ�.
/// </summary>
public sealed class BS_CheckAttackableArea : BehaviorService
{
    /// <summary>
    /// ���� Y �������Դϴ�.
    /// </summary>
    private  float _AreaOffsetY;

    /// <summary>
    /// ���� Z �������Դϴ�.
    /// </summary>
    private  float _AreaOffsetZ;

    /// <summary>
    /// ������ ������
    /// </summary>
    private float _AreaRadius;

    /// <summary>
    /// ������ ������Ʈ ���̾�
    /// </summary>
    private LayerMask _DetectLayer;

    #region �����
    private DrawGizmoSphereInfo _DebugAttackArea;
    #endregion

    public BS_CheckAttackableArea(
        float areaOffsetY,
        float areaOffsetZ,
        float areaRadius,
        LayerMask detectLayer)
    {
        _AreaOffsetY = areaOffsetY;
        _AreaOffsetZ = areaOffsetZ;
        _AreaRadius = areaRadius;
        _DetectLayer = detectLayer;
    }

    public override void ServiceTick()
    {
        // �˻� ������ �߽� ��ġ�� ����մϴ�.
        Vector3 center = behaviorController.transform.position + 
            (behaviorController.transform.up * _AreaOffsetY) +
            (behaviorController.transform.forward * _AreaOffsetZ);

        Collider[] detectCollisions = PhysicsExt.OverlapSphere(out _DebugAttackArea,
            center, _AreaRadius, _DetectLayer, QueryTriggerInteraction.Ignore);

        if(detectCollisions.Length > 0)
        {
            Debug.Log("�÷��̾� ����!");
        }

    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        PhysicsExt.DrawOverlapSphere(_DebugAttackArea);
    }
#endif

}