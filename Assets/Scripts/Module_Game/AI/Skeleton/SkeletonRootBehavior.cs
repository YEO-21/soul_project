using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public sealed class SkeletonRootBehavior : BehaviorSelector
{
    public SkeletonRootBehavior()
    {
        // 플레이어를 감지하지 않았을 경우
        AddBehavior<SkeletonNormalSequencer>();

       
       // 플레이어를 감지한 경우
       AddBehavior<SkeletonAggressiveSelector>();

    }
   

}
