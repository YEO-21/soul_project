using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed partial class SkeletonAttack : MonoBehaviour
{
    [Header("# 장착중인 무기")]
    public WeaponBase m_EquippedWeapon;

    [Header("# 공격 감지 레이어")]
    public LayerMask m_DetectLayer;

    /// <summary>
    /// Skeleton 객체를 나타냅니다.
    /// </summary>
    private EnemySkeleton _Skeleton;

    /// <summary>
    /// 이미 공격된 목표 객체를 기록할 리스트입니다.
    /// 중복 공격을 막기 위하여 사용됩니다.
    /// </summary>
    private List<IDamageable> _AttackedTargets = new();

    /// <summary>
    /// 공격 상태를 나타냅니다.
    /// </summary>
    public bool isAttacking { get; private set; }

    /// <summary>
    /// 공격 방향을 나타냅니다.
    /// </summary>
    public Vector3 attackDirection { get; private set; }

    #region 이벤트
    /// <summary>
    /// 공격 시작 이벤트
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

        // 공격 대상 레이어 설정
        m_EquippedWeapon.InitializeWeapon(m_DetectLayer);

        // 공격 대상 감지 이벤트 바인딩
        m_EquippedWeapon.onDetected += CALLBACK_OnDamageableDetected;
    }

    /// <summary>
    /// 공격을 시작합니다.
    /// </summary>
    public void StartAttack(Vector3 attackDirection)
    {
        if (isAttacking) return;

        // 공격 방향을 기록합니다.
        this.attackDirection = attackDirection;

        isAttacking = true;
        onAttackStarted?.Invoke();
    }

    /// <summary>
    /// 공격 상태를 종료합니다.
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
        // 이미 공격된 객체 리스트에 포함되어 있는 경우 함수 호출 종료.
        if (_AttackedTargets.Contains(newTarget)) return;

        // 공격 처리됨 리스트에 등록합니다.
        _AttackedTargets.Add(newTarget);

        // 감지된 객체(플레이어 캐릭터)에게 피해를 입힙니다.
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
    /// Skeleton 공격 Damage 클래스입니다.
    /// </summary>
    public class SkeletonAttackDamage : DamageBase
    {
        public SkeletonAttackDamage(
            Transform from,
            float damage,
            bool isCriticalDamage) : base(from, damage, isCriticalDamage) { }
    }
}
