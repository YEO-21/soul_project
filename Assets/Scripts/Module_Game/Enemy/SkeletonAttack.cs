using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class SkeletonAttack : MonoBehaviour
{
    [Header("# �������� ����")]
    public WeaponBase m_EquippedWeapon;

    [Header("# ���� ���� ���̾�")]
    public LayerMask m_DetectLayer;

    /// <summary>
    /// Skeleton ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    private EnemySkeleton _Skeleton;

    /// <summary>
    /// �̹� ���ݵ� ��ǥ ��ü�� ����� ����Ʈ�Դϴ�.
    /// �ߺ� ������ ���� ���Ͽ� ���˴ϴ�.
    /// </summary>
    private List<IDamageable> _AttackedTargets = new();

    /// <summary>
    /// ���� ���¸� ��Ÿ���ϴ�.
    /// </summary>
    public bool isAttacking { get; private set; }

    /// <summary>
    /// ���� ������ ��Ÿ���ϴ�.
    /// </summary>
    public Vector3 attackDirection { get; private set; }

    #region �̺�Ʈ
    /// <summary>
    /// ���� ���� �̺�Ʈ
    /// </summary>
    public event System.Action onAttackStarted;
    #endregion

    public void Initialize(EnemySkeleton skeleton)
    {
        _Skeleton = skeleton;

        _Skeleton.animController.onAttackAnimationFinished += CALLBACK_OnAttackAnimationFinished;
        _Skeleton.animController.onAttackAreaCheckStarted += CALLBACK_OnAttackAreaCheckStarted;
        _Skeleton.animController.onAttackAreaCheckFinished += CALLBACK_OnAttackAreaCheckFinished;

        _Skeleton.onHit += CALLBACK_OnHit;

        // ���� ��� ���̾� ����
        m_EquippedWeapon.InitializeWeapon(m_DetectLayer);

        // ���� ��� ���� �̺�Ʈ ���ε�
        m_EquippedWeapon.onDetected += CALLBACK_OnDamageableDetected;
    }

    /// <summary>
    /// ������ �����մϴ�.
    /// </summary>
    public void StartAttack(Vector3 attackDirection)
    {
        if (isAttacking) return;

        // ���� ������ ����մϴ�.
        this.attackDirection = attackDirection;

        isAttacking = true;
        onAttackStarted?.Invoke();
    }

    /// <summary>
    /// ���� ���¸� �����մϴ�.
    /// </summary>
    private void FinishAttackState()
    {
        isAttacking = false;
        _AttackedTargets.Clear();
    }

    private void CALLBACK_OnAttackAnimationFinished()
        => FinishAttackState();

    private void CALLBACK_OnHit(DamageBase damageInstance)
    {
        if (isAttacking)
        {
            isAttacking = false;
        }
    }

    private void CALLBACK_OnAttackAreaCheckStarted()
        => m_EquippedWeapon.StartAttackAreaCheck();

    private void CALLBACK_OnAttackAreaCheckFinished()
        => m_EquippedWeapon.StopAttackAreaCheck();

    private void CALLBACK_OnDamageableDetected(IDamageable newTarget)
    {
        // �̹� ���ݵ� ��ü ����Ʈ�� ���ԵǾ� �ִ� ��� �Լ� ȣ�� ����.
        if (_AttackedTargets.Contains(newTarget)) return;

        // ���� ó���� ����Ʈ�� ����մϴ�.
        _AttackedTargets.Add(newTarget);

        // ������ ��ü(�÷��̾� ĳ����)���� ���ظ� �����ϴ�.
        DamageBase.Hit(
            newTarget, 
            new SkeletonAttackDamage(
                _Skeleton.transform,
                _Skeleton.enemyInfo.m_Atk,
                false));
    }
}


public sealed partial class SkeletonAttack : MonoBehaviour
{
    /// <summary>
    /// Skeleton ���� Damage Ŭ�����Դϴ�.
    /// </summary>
    public class SkeletonAttackDamage : DamageBase
    {
        public SkeletonAttackDamage(
            Transform from,
            float damage,
            bool isCriticalDamage) : base(from, damage, isCriticalDamage) { }
    }
}
