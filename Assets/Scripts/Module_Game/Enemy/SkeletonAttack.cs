using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonAttack : MonoBehaviour
{
    /// <summary>
    /// 공격 상태를 나타냅니다.
    /// </summary>
    public bool isAttacking { get; private set; }

    /// <summary>
    /// 공격 시작 이벤트
    /// </summary>
    public event System.Action onAttackStarted;

    public void Initialize(EnemySkeleton skeleton)
    {
        skeleton.animController.onAttackAnimationFinished += CALLBACK_OnAttackAnimationFinished;
    }


    /// <summary>
    /// 공격을 시작합니다.
    /// </summary>
   public void StartAttack()
    {
        if (isAttacking) return;

        Debug.Log("공격 시작!");

        isAttacking = true;
        onAttackStarted?.Invoke();
    }

    private void CALLBACK_OnAttackAnimationFinished()
    {
        isAttacking = false;
    }

}
