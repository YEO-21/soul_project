using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonAttack : MonoBehaviour
{
    [Header("# 장착중인 무기")]
    public WeaponBase m_EquippedWeapon;

    [Header("# 공격 감지 레이어")]
    public LayerMask m_DetectLayer;

    /// <summary>
    /// 공격 상태를 나타냅니다.
    /// </summary>
    public bool isAttacking { get; private set; }

    /// <summary>
    /// 공격 방향을 나타냅니다.
    /// </summary>
    public Vector3 attackDirection { get; private set; }

    /// <summary>
    /// 공격 시작 이벤트
    /// </summary>
    public event System.Action onAttackStarted;

    public void Initialize(EnemySkeleton skeleton)
    {
        skeleton.animController.onAttackAnimationFinished += CALLBACK_OnAttackAnimationFinished;
        skeleton.animController.onAttackAreaCheckStarted += CALLBACK_OnAttackAreaCheckStarted;
        skeleton.animController.onAttackAreaCheckFinished += CALLBACK_OnAttackAreaCheckFinished;

        skeleton.onHit += CALLABACK_OnHit;
    }


    /// <summary>
    /// 공격을 시작합니다.
    /// </summary>
   public void StartAttack(Vector3 attackDirection)
    {
        if (isAttacking) return;
        
        // 공격 방향을 기록합니다.
        this.attackDirection  = attackDirection;

        isAttacking = true;
        onAttackStarted?.Invoke();
    }

    private void CALLBACK_OnAttackAnimationFinished()
    {
        isAttacking = false;
    }

    private void CALLABACK_OnHit(DamageBase damageInstance)
    {
        if(isAttacking)
        {
            isAttacking = false;
        }
    }

    private void CALLBACK_OnAttackAreaCheckStarted()
        => m_EquippedWeapon.StartAttackAreaCheck();
    private void CALLBACK_OnAttackAreaCheckFinished()
        => m_EquippedWeapon.StopAttackAreaCheck();
}
