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

    private LayerMask _DetectLayer;

    /// <summary>
    /// ���� ���� �˻������� ��Ÿ���ϴ�.
    /// </summary>
    private bool _IsAttackAreaChecking;

    #region �̺�Ʈ
    /// <summary>
    /// ��ü ���� �̺�Ʈ
    /// </summary>
    public event System.Action<IDamageable> onDetected;
    #endregion

    #region �����
    private DrawGizmoSphereInfo _AttackAreaCheckGizmoInfo;
    #endregion

    protected virtual void Update()
    {
        // ���� ������ �˻��մϴ�.
        CheckAttackArea();
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    /// <param name="detectLayer"></param>
    public virtual void InitializeWeapon(LayerMask detectLayer)
    {
        _DetectLayer = detectLayer;
    }

    /// <summary>
    /// ���� ������ �˻��մϴ�.
    /// </summary>
    private void CheckAttackArea()
    {
        // ���� ���� �˻����� �ƴ� ��� ȣ�� ����.
        if (!_IsAttackAreaChecking) return;

        // �߻� ������ ����մϴ�.
        Vector3 checkDirection =
            m_AttackCheckEndPosition.transform.position - 
            m_AttackCheckStartPosition.transform.position;
        checkDirection.Normalize();

        // ���� �ִ� �Ÿ��� ����մϴ�.
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
            // ���ظ� ���� �� �ִ� ������ ��ü�� ����ϴ�.
            IDamageable detectedObject = hitInfo.transform.GetComponent<IDamageable>();

            if (detectedObject != null)
            {
                // ���� �̺�Ʈ �߻�
                onDetected?.Invoke(detectedObject);
            }

#if UNITY_EDITOR
            else
            {
                Debug.LogError($"{hitInfo.transform.gameObject.name} " +
                    $"�� IDamageable �������̽��� �����ϴ� Ŭ������ �������� �ʽ��ϴ�.");
            }
#endif
        }
    }

    /// <summary>
    /// ���� ���� �˻縦 �����մϴ�.
    /// </summary>
    public virtual void StartAttackAreaCheck()
    {
        _IsAttackAreaChecking = true;
    }

    /// <summary>
    /// ���� ���� �˻縦 �����ϴ�.
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
