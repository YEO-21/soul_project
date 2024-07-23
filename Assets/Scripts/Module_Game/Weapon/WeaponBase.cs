using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 객체를 나타내기 위한 클래스입니다.
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    [Header("# 공격 검사 끝점")]
    public Transform m_AttackCheckStartPosition;
    public Transform m_AttackCheckEndPosition;

    [Header("# 공격 영역 크기")]
    public float m_AttackCheckRadius = 0.2f;

    private LayerMask _DetectLayer;

    /// <summary>
    /// 공격 영역 검사중임을 나타냅니다.
    /// </summary>
    private bool _IsAttackAreaChecking;

    #region 이벤트
    /// <summary>
    /// 객체 감지 이벤트
    /// </summary>
    public event System.Action<IDamageable> onDetected;
    #endregion

    #region 디버그
    private DrawGizmoSphereInfo _AttackAreaCheckGizmoInfo;
    #endregion

    protected virtual void Update()
    {
        // 공격 영역을 검사합니다.
        CheckAttackArea();
    }

    /// <summary>
    /// 무기 초기화
    /// </summary>
    /// <param name="detectLayer"></param>
    public virtual void InitializeWeapon(LayerMask detectLayer)
    {
        _DetectLayer = detectLayer;
    }

    /// <summary>
    /// 공격 영역을 검사합니다.
    /// </summary>
    private void CheckAttackArea()
    {
        // 공격 영역 검사중이 아닌 경우 호출 종료.
        if (!_IsAttackAreaChecking) return;

        // 발사 방향을 계산합니다.
        Vector3 checkDirection =
            m_AttackCheckEndPosition.transform.position - 
            m_AttackCheckStartPosition.transform.position;
        checkDirection.Normalize();

        // 감지 최대 거리를 계산합니다.
        float checkMaxDistance = Vector3.Distance(
            m_AttackCheckEndPosition.transform.position,
            m_AttackCheckStartPosition.transform.position);

        Ray ray = new Ray(
            m_AttackCheckStartPosition.transform.position,
            checkDirection);

        if (PhysicsExt.SphereCast(
            out _AttackAreaCheckGizmoInfo,
            ray,
            m_AttackCheckRadius,
            out RaycastHit hitInfo,
            checkMaxDistance,
            _DetectLayer,
            QueryTriggerInteraction.Ignore))
        {
            // 피해를 입을 수 있는 감지된 객체를 얻습니다.
            IDamageable detectedObject = hitInfo.transform.GetComponent<IDamageable>();

            if (detectedObject != null)
            {
                // 감지 이벤트 발생
                onDetected?.Invoke(detectedObject);
            }

#if UNITY_EDITOR
            else
            {
                Debug.LogError($"{hitInfo.transform.gameObject.name} " +
                    $"에 IDamageable 인터페이스를 구현하는 클래스가 존재하지 않습니다.");
            }
#endif
        }
    }

    /// <summary>
    /// 공격 영역 검사를 시작합니다.
    /// </summary>
    public virtual void StartAttackAreaCheck()
    {
        _IsAttackAreaChecking = true;
    }

    /// <summary>
    /// 공격 영역 검사를 끝냅니다.
    /// </summary>
    public virtual void StopAttackAreaCheck()
    {
        _IsAttackAreaChecking = false;
    }


#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (_IsAttackAreaChecking)
            PhysicsExt.DrawGizmoSphere(_AttackAreaCheckGizmoInfo);
    }
#endif


}
