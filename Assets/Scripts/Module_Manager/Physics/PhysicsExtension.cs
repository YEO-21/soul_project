using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �� �׸��⿡ �ʿ��� ������ ��� Ŭ����
/// </summary>
public class DrawGizmoLineInfo
{
    /// <summary>
    /// �⺻ ����
    /// </summary>
    public Color defaultColor { get; set; } = new Color(0.0f, 1.0f, 0.0f, 1.0f);

    /// <summary>
    /// �����Ǿ��� ��� ������ų ����
    /// </summary>
    public Color detectedColor { get; set; } = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    /// <summary>
    /// ���� ����
    /// </summary>
    public bool isHit { get; set; }

    /// <summary>
    /// ���� ��ġ
    /// </summary>
    public Vector3 start { get; set; }

    /// <summary>
    /// �� ��ġ
    /// </summary>
    public Vector3 end { get; set; }

    /// <summary>
    /// �׸� ���� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public Color drawColor => isHit ? detectedColor : defaultColor;
}

/// <summary>
/// ������ ��ü �׸��⿡ �ʿ��� ������ ��� Ŭ����
/// </summary>
public class DrawGizmoSphereInfo : DrawGizmoLineInfo
{ 
    /// <summary>
    /// ��ü�� ������
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

        // �׸� ���� ����
        Gizmos.color = drawInfo.drawColor;

        // �� �׸���
        Gizmos.DrawLine(drawInfo.start, drawInfo.end);
    }

    public static void DrawGizmoSphere(in DrawGizmoSphereInfo drawInfo)
    {
        if (drawInfo == null) return;

        // ���� ����
        Gizmos.color = drawInfo.drawColor;

        // ���� ��ġ���� �� ��ġ���� �׸��ϴ�.
        Gizmos.DrawWireSphere(drawInfo.start, drawInfo.radius); // ���� ��ġ ��ü �׸���
        Gizmos.DrawLine(drawInfo.start, drawInfo.end);
        Gizmos.DrawWireSphere(drawInfo.end, drawInfo.radius); // �� ��ġ ��ü �׸���
    }

    public static void DrawOverlapSphere(in DrawGizmoSphereInfo drawInfo)
    {
        if (drawInfo == null) return;

        Gizmos.color = drawInfo.drawColor;
        Gizmos.DrawWireSphere(drawInfo.start, drawInfo.radius);
    }
}
