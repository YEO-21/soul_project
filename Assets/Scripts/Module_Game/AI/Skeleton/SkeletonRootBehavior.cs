using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public sealed class SkeletonRootBehavior : BehaviorSequencer
{
    public SkeletonRootBehavior()
    {
        SkeletonBehaviorController bhController = behaviorController as SkeletonBehaviorController;

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
   

}
