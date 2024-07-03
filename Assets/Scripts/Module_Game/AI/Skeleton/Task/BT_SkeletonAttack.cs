using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BT_SkeletonAttack : RunnableBehavior
{
    private string _IsAttackableKey;

    public BT_SkeletonAttack(string isAttackableKey)
    {
        _IsAttackableKey = isAttackableKey;
    }


    public override IEnumerator OnBehaivorStarted()
    {
        EnemySkeleton skeleton = (behaviorController as EnemyBehaviorController).
            ownerCharacter as EnemySkeleton;

        // 공격 시작
        skeleton.attack.StartAttack();

        yield return new WaitUntil(() => skeleton.attack.isAttacking);
        //yield return new WaitWhile(() => skeleton.attack.isAttacking);

        behaviorController.SetKey(_IsAttackableKey, false);

        isSucceeded = true;
        yield return null;


    }


}
