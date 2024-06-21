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

    [Header("# 감지 레이어")]
    public LayerMask m_DetectLayer;

    #region 디버그
    private DrawGizmoSphereInfo _AttackAreaCheckGizmoInfo;

    #endregion


    protected virtual void Update()
    {
        // 공격 영역을 검사합니다.
        CheckAttackArea();
    }
    
    /// <summary>
   /// 공격 영역을 검사합니다.
   /// </summary>
    private void CheckAttackArea()
    {

        Vector3 checkDirectioin = 
            m_AttackCheckEndPosition.position - m_AttackCheckStartPosition.position;

        checkDirectioin.Normalize();

        // 감지 최대 거리를 계산합니다.
        float checkMaxDistance = Vector3.Distance(
            m_AttackCheckEndPosition.position, m_AttackCheckStartPosition.position);

        Ray ray = new Ray(m_AttackCheckStartPosition.position,
            checkDirectioin);

        if (PhysicsExt.SphereCast(
            out _AttackAreaCheckGizmoInfo,
            ray,
            m_AttackCheckRadius,
            out RaycastHit hitInfo,
            checkMaxDistance,
            m_DetectLayer,
            QueryTriggerInteraction.Ignore))
        {
            // 어떠한 처리를 
        }
    }

    /// <summary>
    /// 공격 영역 검사를 시작합니다.
    /// </summary>
    public void STARTattackAreaCheck()
    {

    }

    /// <summary>
    /// 공격 영역 검사를 끝냅니다.
    /// </summary>
    public void StopAttackAreaCheck()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        PhysicsExt.DrawGizmoSphere(_AttackAreaCheckGizmoInfo);
    }

}
