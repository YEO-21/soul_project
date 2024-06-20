using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public sealed class SkeletonRootBehavior : BehaviorSequencer
{
    public SkeletonRootBehavior()
    {
        SkeletonBehaviorController bhController = behaviorController as SkeletonBehaviorController;

        AddBehavior(() => new BT_GetRandomPositionInNavigableRadius(
            SkeletonBehaviorController.KEY_TARGETPOSITION,
            SkeletonBehaviorController.KEY_MAXMOVEDISTANCE,
            SkeletonBehaviorController.KEY_SPAWNPOSITION));
    }


    // 이동할 랜덤한 위치 설정
    // 설정된 위치로 이동
    // 대기

}
