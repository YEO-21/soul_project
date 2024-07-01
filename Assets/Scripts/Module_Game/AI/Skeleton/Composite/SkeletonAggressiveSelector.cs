using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public sealed class SkeletonAggressiveSelector : BehaviorSequencer
{
    public SkeletonAggressiveSelector()
    {
        // �÷��̾ ���� ���� ������ �������� ���� ���
        AddBehavior<SkeletonTrackingSequencer>();

       
    }


    public override bool OnInitialized(BehaviorController behaviorController)
    {
        base.OnInitialized(behaviorController);

        // �������� �������� Ȯ���մϴ�.
        bool isAggressiveState = behaviorController.GetKey<bool>(
            EnemyBehaviorController.KEY_ISAGGRESSIVESTATE);

        // �������� ���¶�� �ʱ�ȭ ����
        return isAggressiveState;
    }

}
