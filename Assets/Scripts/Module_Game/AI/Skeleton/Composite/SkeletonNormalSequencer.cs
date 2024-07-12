using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonNormalSequencer : BehaviorSequencer
{
    public SkeletonNormalSequencer()
    {


        // 이동할 랜덤한 위치 설정
        AddBehavior(() => new BT_GetRandomPositionInNavigableRadius(
            SkeletonBehaviorController.KEY_TARGETPOSITION,
            SkeletonBehaviorController.KEY_MAXMOVEDISTANCE,
            SkeletonBehaviorController.KEY_SPAWNPOSITION));

        // 설정된 위치로 이동
        AddBehavior(() => new BT_MoveTo(SkeletonBehaviorController.KEY_TARGETPOSITION));

        // 대기
        AddBehavior(() => new BT_Wait(Random.Range(0.0f, 3.0f)));

       
    }

    public override bool OnInitialized(BehaviorController behaviorController)
    {
        base.OnInitialized(behaviorController);

        // 공격적인 상태인지 확인합니다.
       bool isAggressiveState =  behaviorController.GetKey<bool>(
           EnemyBehaviorController.KEY_ISAGGRESSIVESTATE);

        
        // 공격적인 상태가 아닌 경우 초기화 성공
        return !isAggressiveState;
    }



}
