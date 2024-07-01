using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public sealed class SkeletonAggressiveSelector : BehaviorSequencer
{
    public SkeletonAggressiveSelector()
    {
        // 플레이어가 공격 가능 영역에 존재하지 않은 경우
        AddBehavior<SkeletonTrackingSequencer>();

       
    }


    public override bool OnInitialized(BehaviorController behaviorController)
    {
        base.OnInitialized(behaviorController);

        // 공격적인 상태인지 확인합니다.
        bool isAggressiveState = behaviorController.GetKey<bool>(
            EnemyBehaviorController.KEY_ISAGGRESSIVESTATE);

        // 공격적인 상태라면 초기화 성공
        return isAggressiveState;
    }

}
