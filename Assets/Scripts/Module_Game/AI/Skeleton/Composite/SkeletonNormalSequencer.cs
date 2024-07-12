using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonNormalSequencer : BehaviorSequencer
{
    public SkeletonNormalSequencer()
    {


        // �̵��� ������ ��ġ ����
        AddBehavior(() => new BT_GetRandomPositionInNavigableRadius(
            SkeletonBehaviorController.KEY_TARGETPOSITION,
            SkeletonBehaviorController.KEY_MAXMOVEDISTANCE,
            SkeletonBehaviorController.KEY_SPAWNPOSITION));

        // ������ ��ġ�� �̵�
        AddBehavior(() => new BT_MoveTo(SkeletonBehaviorController.KEY_TARGETPOSITION));

        // ���
        AddBehavior(() => new BT_Wait(Random.Range(0.0f, 3.0f)));

       
    }

    public override bool OnInitialized(BehaviorController behaviorController)
    {
        base.OnInitialized(behaviorController);

        // �������� �������� Ȯ���մϴ�.
       bool isAggressiveState =  behaviorController.GetKey<bool>(
           EnemyBehaviorController.KEY_ISAGGRESSIVESTATE);

        
        // �������� ���°� �ƴ� ��� �ʱ�ȭ ����
        return !isAggressiveState;
    }



}
