using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// �ð� ���� Ŭ�����Դϴ�.
/// </summary>
public class SightSense : SenseBase
{
    /// <summary>
    /// �þ߰��� ��Ÿ���ϴ�.
    /// </summary>
    public float sightMaxAngle { get; set; }

    /// <summary>
    /// �þ� �Ÿ��� ��Ÿ���ϴ�.
    /// </summary>
    public float sightRadius { get; set; }

    /// <summary>
    /// ���� ���̸� ��Ÿ���ϴ�.
    /// </summary>
    public float detectHeight { get; set; }

    /// <summary>
    /// ���� ���� ��Ȱ��ȭ���� �ɸ��� �ð�
    /// </summary>
    public float maxDetectionTime { get; set; } = 5.0f;

    /// <summary>
    /// ������ ��ü�� ���̾ ��Ÿ���ϴ�.
    /// </summary>
    public LayerMask detectLayer { get; set; }

    /// <summary>
    /// �ð� ���� ��ü Ȱ��ȭ ����
    /// </summary>
    public bool isSIghtSenseEnabled { get; private set; }

    /// <summary>
    /// ������ ��ü�� �ð��� ����մϴ�.
    /// </summary>
    private Dictionary<GameObject, float> _DetectedTargets = new();


    #region �̺�Ʈ
    public event System.Action<GameObject> onTargetDetected;
    public event System.Action<GameObject> onTargetLost;
    #endregion

    #region �����
    private DrawGizmoLineInfo[] _DebugSightRayInfos;
    #endregion


    public override void OnSenseUpdated()
    {
        base.OnSenseUpdated();

        UpdateDetectState();

        if (isSIghtSenseEnabled = IsSightSenseEnable())
            UpdateSightSense();
    }

    /// <summary>
    /// �ð� ���� ��� Ȱ��ȭ ���� ���θ� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    private bool IsSightSenseEnable()
    {
        Vector3 center = behaviorController.transform.position + (Vector3.up * detectHeight);

        // ���� ������ ���� ���ο� ������ �� �ִ� ��ü�� �����ϴ��� �˻��մϴ�.
        Collider[] detectedCollisions = Physics.OverlapSphere(
            center,
            sightRadius,
            detectLayer,
            QueryTriggerInteraction.Ignore);

        // ���� ������ ��ü�� �����Ѵٸ�
        return detectedCollisions.Length > 0;
    }

    /// <summary>
    /// �ð� ���� 
    /// </summary>
    private void UpdateSightSense()
    {
        // Ray ����
        int raycastCount = ((int)sightMaxAngle / 5) + 1;

        if (_DebugSightRayInfos == null)
            _DebugSightRayInfos = new DrawGizmoLineInfo[raycastCount];

        // ���� ����� ���Ͽ� �� ������ ����ϴ�.
        Vector3 forwardDirection = behaviorController.transform.forward;

        // �� ���⿡ ���� Yaw ȸ������ ����մϴ�.
        float forwardYawAngle = Mathf.Atan2(
            forwardDirection.x,
            forwardDirection.z) * Mathf.Rad2Deg;

        // Ray �߻� ���� ������ ����մϴ�.
        // ���ʺ��� �ð� �������� �߻��ŵ�ϴ�.
        float startYawAngle = forwardYawAngle - (sightMaxAngle * 0.5f);


        for (int i = 0; i < raycastCount; ++i)
        {
            if (_DebugSightRayInfos[i] == null)
                _DebugSightRayInfos[i] = new DrawGizmoLineInfo();

            float angle = (startYawAngle + (i * 5)) * Mathf.Deg2Rad;
            Vector3 origin = behaviorController.transform.position +
                Vector3.up * detectHeight;

            Vector3 direction = new Vector3(
                Mathf.Sin(angle),
                forwardDirection.y,
                Mathf.Cos(angle));

            Ray ray = new Ray(origin, direction);
            if (PhysicsExt.Raycast(
                out _DebugSightRayInfos[i],
                ray,
                out RaycastHit hit,
                sightRadius,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore))
            {
                // ������ ������Ʈ
                GameObject detectedObject = hit.transform.gameObject;

                // ������ ������Ʈ ���̾�
                int detectedObjectLayer = 1 << detectedObject.layer;

                // detectLayer �� ������ ���̾ ���� ������Ʈ�� ������ ���
                if ((detectLayer.value & detectedObjectLayer) != 0)
                    DetectTarget(detectedObject);
            }
        }
    }

    /// <summary>
    /// ���� ���¸� �����մϴ�.
    /// </summary>
    private void UpdateDetectState()
    {
        // ���� �ð�
        float currentTime = Time.time;

        // ���� �ð��� �ʰ��� ������Ʈ���� ��� ���� ����Ʈ
        List<GameObject> lostTargets = new();

        foreach(KeyValuePair<GameObject, float> detectedTarget in _DetectedTargets)
        {
            // ��ü�� ������ �ð��� ����ϴ�.
            float detectedTime = detectedTarget.Value;

            // �ִ� ���� �ð��� ���� ���
            if (detectedTime + maxDetectionTime <= currentTime)
                lostTargets.Add(detectedTarget.Key);
        }

        foreach(GameObject lostTarget in lostTargets)
        {
            _DetectedTargets.Remove(lostTarget);
            onTargetLost?.Invoke(lostTarget);
        }
    }

    private void DetectTarget(GameObject target)
    {
        if (_DetectedTargets.ContainsKey(target))
        {
            _DetectedTargets[target] = Time.time;
            onTargetDetected?.Invoke(target);
        }
        else
        {
            _DetectedTargets.Add(target, Time.time);
        }
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Vector3 center = behaviorController.transform.position + Vector3.up * detectHeight;
        Vector3 forward = behaviorController.transform.forward;

        Color drawColor = Color.green;
        drawColor.a = 0.2f;

        Handles.color = drawColor;
        Handles.DrawSolidArc(center, Vector3.up, forward, sightMaxAngle * 0.5f, sightRadius);
        Handles.DrawSolidArc(center, Vector3.down, forward, sightMaxAngle * 0.5f, sightRadius);

        if (isSIghtSenseEnabled)
        {
            if (_DebugSightRayInfos != null)
            {
                foreach(DrawGizmoLineInfo gizmoLineInfo in _DebugSightRayInfos)
                    PhysicsExt.DrawGizmoLine(gizmoLineInfo);
            }
        }

        if (_DetectedTargets.Count > 0)
        {
            foreach(KeyValuePair<GameObject, float> target in _DetectedTargets)
            {
                // ������ ��ǥ ������Ʈ ��ġ
                Vector3 targetPosition = target.Key.transform.position + 
                    Vector3.up * detectHeight;

                Gizmos.color = Color.green;
                Gizmos.DrawLine(center, targetPosition);
                Gizmos.DrawWireSphere(targetPosition, 0.5f);
            }
        }
    }
#endif
}
