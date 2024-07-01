

using UnityEngine;
/// <summary>
/// 적 캐릭터의 공격 가능 영역을 검사하기 위한 서비스입니다.
/// </summary>
public sealed class BS_CheckAttackableArea : BehaviorService
{
    /// <summary>
    /// 영역 Y 오프셋입니다.
    /// </summary>
    private  float _AreaOffsetY;

    /// <summary>
    /// 영역 Z 오프셋입니다.
    /// </summary>
    private  float _AreaOffsetZ;

    /// <summary>
    /// 영역의 반지름
    /// </summary>
    private float _AreaRadius;

    /// <summary>
    /// 감지할 오브젝트 레이어
    /// </summary>
    private LayerMask _DetectLayer;

    #region 디버그
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
        // 검사 영역의 중심 위치를 계산합니다.
        Vector3 center = behaviorController.transform.position + 
            (behaviorController.transform.up * _AreaOffsetY) +
            (behaviorController.transform.forward * _AreaOffsetZ);

        Collider[] detectCollisions = PhysicsExt.OverlapSphere(out _DebugAttackArea,
            center, _AreaRadius, _DetectLayer, QueryTriggerInteraction.Ignore);

        if(detectCollisions.Length > 0)
        {
            Debug.Log("플레이어 감지!");
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