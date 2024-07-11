using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonAttack : MonoBehaviour
{
    /// <summary>
    /// ���� ���¸� ��Ÿ���ϴ�.
    /// </summary>
    public bool isAttacking { get; private set; }

    /// <summary>
    /// ���� ������ ��Ÿ���ϴ�.
    /// </summary>
    public Vector3 attackDirection { get; private set; }

    /// <summary>
    /// ���� ���� �̺�Ʈ
    /// </summary>
    public event System.Action onAttackStarted;

    public void Initialize(EnemySkeleton skeleton)
    {
        skeleton.animController.onAttackAnimationFinished += CALLBACK_OnAttackAnimationFinished;
        skeleton.onHit += CALLABACK_OnHit;
    }


    /// <summary>
    /// ������ �����մϴ�.
    /// </summary>
   public void StartAttack(Vector3 attackDirection)
    {
        if (isAttacking) return;
        
        // ���� ������ ����մϴ�.
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

}
