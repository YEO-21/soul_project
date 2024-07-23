using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BT_SkeletonAttack : RunnableBehavior
{
    private string _IsAttackableKey;
    private string _PlayerCharacterKey;

    public BT_SkeletonAttack(string isAttackableKey, string playerCharacterKey)
    {
        _IsAttackableKey = isAttackableKey;
        _PlayerCharacterKey = playerCharacterKey;
    }

    public override IEnumerator OnBehaviorStarted()
    {
        EnemySkeleton skeleton = (behaviorController as EnemyBehaviorController).
            ownerCharacter as EnemySkeleton;

        PlayerCharacter playerCharacter =
            behaviorController.GetKey<PlayerCharacter>(_PlayerCharacterKey);

        // 플레이어 캐릭터로 향하는 방향
        Vector3 direction = playerCharacter.transform.position - skeleton.transform.position;
        direction.y = 0.0f;
        direction.Normalize();
        //direction = direction.normalized;

        // 공격 시작
        skeleton.attack.StartAttack(direction);

        yield return new WaitUntil(() => skeleton.attack.isAttacking);
        yield return new WaitWhile(() => skeleton.attack.isAttacking);

        behaviorController.SetKey(_IsAttackableKey, false);

        isSucceeded = true;
        yield return null;
    }
}
