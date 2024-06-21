using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��ü�� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    [Header("# ���� �˻� ����")]
    public Transform m_AttackCheckStartPosition;
    public Transform m_AttackCheckEndPosition;

    [Header("# ���� ���� ũ��")]
    public float m_AttackCheckRadius = 0.2f;

    [Header("# ���� ���̾�")]
    public LayerMask m_DetectLayer;

    #region �����
    private DrawGizmoSphereInfo _AttackAreaCheckGizmoInfo;

    #endregion


    protected virtual void Update()
    {
        // ���� ������ �˻��մϴ�.
        CheckAttackArea();
    }
    
    /// <summary>
   /// ���� ������ �˻��մϴ�.
   /// </summary>
    private void CheckAttackArea()
    {

        Vector3 checkDirectioin = 
            m_AttackCheckEndPosition.position - m_AttackCheckStartPosition.position;

        checkDirectioin.Normalize();

        // ���� �ִ� �Ÿ��� ����մϴ�.
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
            // ��� ó���� 
        }
    }

    /// <summary>
    /// ���� ���� �˻縦 �����մϴ�.
    /// </summary>
    public void STARTattackAreaCheck()
    {

    }

    /// <summary>
    /// ���� ���� �˻縦 �����ϴ�.
    /// </summary>
    public void StopAttackAreaCheck()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        PhysicsExt.DrawGizmoSphere(_AttackAreaCheckGizmoInfo);
    }

}
