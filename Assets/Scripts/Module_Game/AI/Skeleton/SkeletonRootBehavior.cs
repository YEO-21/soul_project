using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public sealed class SkeletonRootBehavior : BehaviorSequencer
{
    public SkeletonRootBehavior()
    {
        SkeletonBehaviorController bhController = behaviorController as SkeletonBehaviorController;

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
   

}
