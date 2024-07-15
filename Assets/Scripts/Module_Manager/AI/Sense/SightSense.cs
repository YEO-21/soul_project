using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 시각 감지 클래스입니다.
/// </summary>
public class SightSense : SenseBase
{
    /// <summary>
    /// 시야각을 나타냅니다.
    /// </summary>
    public float sightMaxAngle { get; set; }

    /// <summary>
    /// 시야 거리를 나타냅니다.
    /// </summary>
    public float sightRadius { get; set; }

    /// <summary>
    /// 감지 높이를 나타냅니다.
    /// </summary>
    public float detectHeight { get; set; }

    /// <summary>
    /// 감지 상태 비활성화까지 걸리는 시간
    /// </summary>
    public float maxDetectionTime { get; set; } = 5.0f;

    /// <summary>
    /// 감지할 객체의 레이어를 나타냅니다.
    /// </summary>
    public LayerMask detectLayer { get; set; }

    /// <summary>
    /// 시각 감지 객체 활성화 여부
    /// </summary>
    public bool isSightSenseEnabled { get; set; }

    /// <summary>
    /// 감지된 객체와 시간을 기록합니다.
    /// </summary>
    private Dictionary<GameObject, float> _DetectedTargets = new();

    #region 디버그
    private DrawGizmoLineInfo[] _DebugSightRayInfos;
    #endregion

    #region 이벤트
    public event System.Action<GameObject> onTargetDetected;
    public event System.Action<GameObject> onTargetLost;
    #endregion
    public override void OnSenseUpdated()
    {
        base.OnSenseUpdated();

        UpdateDetectState();

        if(isSightSenseEnabled = IsSightSenseEnable())
            UpdateSightSense();
        
    }

    /// <summary>
    /// 시각 감지 기능 활성화 간의 여부를 반환합니다.
    /// </summary>
    /// <returns></returns>
    private bool IsSightSenseEnable()
    {
        Vector3 center = behaviorController.transform.position + (Vector3.up * detectHeight);

        // 감지 가능한 영역 내부에 감지할 수 있는 객체가 존재하는지 검사합니다.
        Collider[] detectedCollisions = Physics.OverlapSphere(
           center, sightRadius, detectLayer,
           QueryTriggerInteraction.Ignore);

        // 감지 가능한 객체가 존재한다면
        return detectedCollisions.Length > 0;
    }

    /// <summary>
    /// 시각 감지
    /// </summary>
    private void UpdateSightSense()
    {
        // Ray 개수
        int raycastCount = ((int)sightMaxAngle / 5) + 1;

        if (_DebugSightRayInfos == null)
            _DebugSightRayInfos = new DrawGizmoLineInfo[raycastCount];

        #region YeoStyle
        //// Ray 개수
        //int raycastCount = ((int)sightMaxAngle / 5) + 1;

        //// 부채꼴 각도를 Ray 로 나눈 각도
        //float dividedAngle = sightMaxAngle / raycastCount;

        //// 절반 각도
        //float halfAngle = sightMaxAngle * 0.5f;

        //DrawGizmoLineInfo drawInfo;


        //Ray[] Rays = new Ray[raycastCount];


        //for (int i = 0; i <raycastCount; ++i)
        //{
        //    // 직각 삼각형 밑변 길이
        //    float triangleDown = Mathf.Cos(halfAngle * Mathf.Deg2Rad) * sightRadius;

        //    // 직각 삼각형 높이
        //    float triangleHeight = Mathf.Tan(((halfAngle - (i * dividedAngle)) * Mathf.Deg2Rad)) * triangleDown;

        //    // 새로운 삼각형 지점
        //    Vector3 newLocation = center + behaviorController.transform.forward * triangleDown - behaviorController.transform.right * (triangleHeight);

        //    // 부채꼴 영역 내의 각 방향들을 얻고 정규화합니다.
        //    Vector3 direction = (newLocation - center);
        //    direction.Normalize();

        //    // ray를 얻습니다.
        //    Ray ray = new Ray(center, direction);

        //    // 레이 캐스트 진행
        //    bool isRayCastCheck = PhysicsExt.Raycast(out drawInfo, ray, out RaycastHit hitInfo, sightRadius, detectLayer);

        //    // 디버깅
        //    Debug.Log("isRayCastCheck = " + isRayCastCheck);

        //    // 레이 캐스트 디버깅
        //    Debug.DrawRay(center, direction * sightRadius);

        //}
        #endregion

        // 각도 계산을 위하여 앞 방향을 얻습니다.
        Vector3 forwardDirection = behaviorController.transform.forward;

        // 앞 방향에 대한 Yaw 회전각을 계산합니다.
        float forwardYawAngle = Mathf.Atan2(forwardDirection.x, forwardDirection.z) * Mathf.Rad2Deg;

        // Ray 발사 시작 각도를 계산합니다.
        // 왼쪽부터 시계 방향으로 발사시킵니다.
        float startYawAngle = forwardYawAngle - (sightMaxAngle * 0.5f);


        for (int i = 0; i < raycastCount; ++i)
        {
            if (_DebugSightRayInfos[i] == null)
                _DebugSightRayInfos[i] = new DrawGizmoLineInfo();


            float angle = (startYawAngle + (i * 5)) * Mathf.Deg2Rad;
            Vector3 origin = behaviorController.transform.position + Vector3.up * detectHeight;

            Vector3 direction = new Vector3(
                Mathf.Sin(angle), forwardDirection.y, Mathf.Cos(angle));

            Ray ray = new Ray(origin, direction);
            Debug.DrawRay(origin, direction * sightRadius, Color.red);

            if (PhysicsExt.Raycast(
                out _DebugSightRayInfos[i],
                ray,
                out RaycastHit hit,
                sightRadius,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore))
            {
                // 감지된 오브젝트
                GameObject detectedObject = hit.transform.gameObject;

                // 감지된 오브젝트 레이어
                int detectedObjectLayer =  1 << detectedObject.layer;

                // detectLayer 로 설정된 레이어를 갖는 오브젝트가 감지된 경우
                if((detectLayer.value & detectedObjectLayer) != 0)
                {
                    DetectTarget(detectedObject);
                }
            }


        }
    }
      
    /// <summary>
    /// 감지 상태를 갱신합니다.
    /// </summary>
    private void UpdateDetectState()
    {
        // 현재 시간
        float currentTime = Time.time;

        // 감지 시간이 초과된 오브젝트들을 담기 위한 리스트
        List<GameObject> lostTargets = new();

        foreach(KeyValuePair<GameObject, float> detectedObject in _DetectedTargets)
        {
            // 객체의 감지된 시간을 얻습니다.
            float detectedTime = detectedObject.Value;

            // 최대 감지 시간이 지난 경우
            if(detectedTime + maxDetectionTime <= currentTime)
            {
                lostTargets.Add(detectedObject.Key);
            }
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

        Handles.DrawSolidArc(center, Vector3.up, forward, sightMaxAngle*0.5f, sightRadius);
        Handles.DrawSolidArc(center, Vector3.down, forward, sightMaxAngle*0.5f, sightRadius);

        if(isSightSenseEnabled)
        {
            if(_DebugSightRayInfos != null)
            {
                foreach(DrawGizmoLineInfo gizmoLineInfo in _DebugSightRayInfos)
                {
                    PhysicsExt.DrawGizmoLine(gizmoLineInfo);
                }
            }
        }

        if(_DetectedTargets.Count > 0)
        {
            foreach(KeyValuePair<GameObject, float> target in _DetectedTargets)
            {
                // 감지된 목표 오브젝트 위치
                Vector3 targetPosition = target.Key.transform.position + Vector3.up * detectHeight;


                Gizmos.color = Color.green;
                Gizmos.DrawLine(center, targetPosition);
                Gizmos.DrawWireSphere(targetPosition, 0.5f);
            }
        }

    }
#endif

}
