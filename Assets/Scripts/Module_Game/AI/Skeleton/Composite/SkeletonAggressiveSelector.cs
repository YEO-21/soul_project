using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonAggressiveSelector : BehaviorSelector
{
    public SkeletonAggressiveSelector()
    {
        AddBehavior<SkeletonAttackSelector>();

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
