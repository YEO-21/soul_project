using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���̷��� �� ĳ������ �ൿ ���� ������Ʈ�Դϴ�.
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
