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


    // �̵��� ������ ��ġ ����
    // ������ ��ġ�� �̵�
    // ���

}
