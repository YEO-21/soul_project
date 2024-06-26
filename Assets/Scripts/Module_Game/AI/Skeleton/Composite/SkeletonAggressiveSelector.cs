using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public sealed class SkeletonAggressiveSelector : BehaviorSequencer
{
    public SkeletonAggressiveSelector()
    {
        // 플레이어 캐릭터 위치를 목표 위치로 설정합니다.
        AddBehavior(() => new BT_TargetPositionToPlayerPosition(
            SkeletonBehaviorController.KEY_PLAYERCHARACTER,
            SkeletonBehaviorController.KEY_TARGETPOSITION));

        // 목표 위치로 이동합니다.
        AddBehavior(() => new BT_MoveTo(SkeletonBehaviorController.KEY_TARGETPOSITION, false));

        // 대기
        AddBehavior(() => new BT_Wait(0.5f));
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
