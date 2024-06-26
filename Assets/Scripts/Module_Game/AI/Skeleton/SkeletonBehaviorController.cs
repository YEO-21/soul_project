using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 스켈레톤 적 캐릭터의 행동 제어 컴포넌트입니다.
/// </summary>
public sealed class SkeletonBehaviorController : EnemyBehaviorController
{
    protected override void Awake()
    {
        base.Awake();

        SetKey(KEY_MAXMOVEDISTANCE, 10.0f);
    }


    private void Start()
    {
        StartBehaivor<SkeletonRootBehavior>();
        
    }





}
