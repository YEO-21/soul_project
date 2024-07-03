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
    /// ���� ���� �̺�Ʈ
    /// </summary>
    public event System.Action onAttackStarted;

    public void Initialize(EnemySkeleton skeleton)
    {
        skeleton.animController.onAttackAnimationFinished += CALLBACK_OnAttackAnimationFinished;
    }


    /// <summary>
    /// ������ �����մϴ�.
    /// </summary>
   public void StartAttack()
    {
        if (isAttacking) return;

        Debug.Log("���� ����!");

        isAttacking = true;
        onAttackStarted?.Invoke();
    }

    private void CALLBACK_OnAttackAnimationFinished()
    {
        isAttacking = false;
    }

}
