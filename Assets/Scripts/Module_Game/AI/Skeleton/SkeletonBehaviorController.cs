using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 스켈레톤 적 캐릭터의 행동 제어 컴포넌트입니다.
/// </summary>
public sealed class SkeletonBehaviorController : EnemyBehaviorController
{
    private void Start()
    {
        StartBehaivor<SkeletonRootBehavior>();
        
    }





}
