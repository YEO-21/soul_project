using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 디버깅용 선 그리기에 필요한 정보를 담는 클래스
/// </summary>
public class DrawGizmoLineInfo
{
    /// <summary>
    /// 기본 색상
    /// </summary>
    public Color defaultColor { get; set; } = new Color(0.0f, 1.0f, 0.0f, 1.0f);

    /// <summary>
    /// 감지되었을 경우 설정시킬 색상
    /// </summary>
    public Color detectedColor { get; set; } = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    /// <summary>
    /// 감지 여부
    /// </summary>
    public bool isHit { get; set; }

    /// <summary>
    /// 시작 위치
    /// </summary>
    public Vector3 start { get; set; }

    /// <summary>
    /// 끝 위치
    /// </summary>
    public Vector3 end { get; set; }

    /// <summary>
    /// 그릴 색상에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public Color drawColor => isHit ? detectedColor : defaultColor;
}

/// <summary>
/// 디버깅용 구체 그리기에 필요한 정보를 담는 클래스
/// </summary>
public class DrawGizmoSphereInfo : DrawGizmoLineInfo
{ 
    /// <summary>
    /// 구체의 반지름
    /// </summary>
    public float radius { get; set; }
}



public static class PhysicsExt
{
    public static bool Raycast(
        out DrawGizmoLineInfo drawInfo,
        Ray ray, 
        out RaycastHit hitInfo, 
        float maxDistance, 
        int layerMask, 
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        bool result = Physics.Raycast(
            ray,
            out hitInfo,
            maxDistance,
            layerMask,
            queryTriggerInteraction);

        drawInfo = new DrawGizmoLineInfo()
        {
            start = ray.origin,
            end = result ? (hitInfo.point) : (ray.origin + ray.direction * maxDistance),
            isHit = result
        };

        return result;
    }

    public static bool SphereCast(
        out DrawGizmoSphereInfo drawInfo,
        Ray ray, 
        float radius, 
        out RaycastHit hitInfo, 
        float maxDistance, 
        int layerMask, 
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        bool result = Physics.SphereCast(ray,
            radius,
            out hitInfo,
            maxDistance,
            layerMask,
            queryTriggerInteraction);

        drawInfo = new DrawGizmoSphereInfo()
        {
            isHit = result,
            start = ray.origin,
            end = result ? 
                ray.origin + ray.direction * hitInfo.distance :
                ray.origin + ray.direction * maxDistance,
            radius = radius,
        };

        return result;
    }

    public static Collider[] OverlapSphere(
        out DrawGizmoSphereInfo info,
        Vector3 center, float radius, int layer,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        Collider[] detectCollisions = Physics.OverlapSphere(
             center, radius, layer, 
             queryTriggerInteraction);

        info = new DrawGizmoSphereInfo
        {
            isHit = detectCollisions.Length > 0,
            start = center,
            end = center,
            radius = radius
        };

        return detectCollisions;
    }


    public static void DrawGizmoLine(in DrawGizmoLineInfo drawInfo)
    {
        if (drawInfo == null) return;

        // 그릴 색상 지정
        Gizmos.color = drawInfo.drawColor;

        // 선 그리기
        Gizmos.DrawLine(drawInfo.start, drawInfo.end);
    }

    public static void DrawGizmoSphere(in DrawGizmoSphereInfo drawInfo)
    {
        if (drawInfo == null) return;

        // 색상 지정
        Gizmos.color = drawInfo.drawColor;

        // 시작 위치부터 끝 위치까지 그립니다.
        Gizmos.DrawWireSphere(drawInfo.start, drawInfo.radius); // 시작 위치 구체 그리기
        Gizmos.DrawLine(drawInfo.start, drawInfo.end);
        Gizmos.DrawWireSphere(drawInfo.end, drawInfo.radius); // 끝 위치 구체 그리기
    }

    public static void DrawOverlapSphere(in DrawGizmoSphereInfo drawInfo)
    {
        if (drawInfo == null) return;

        Gizmos.color = drawInfo.drawColor;
        Gizmos.DrawWireSphere(drawInfo.start, drawInfo.radius);
    }
}
