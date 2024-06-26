using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public sealed class SkeletonAggressiveSelector : BehaviorSequencer
{
    public SkeletonAggressiveSelector()
    {
        // �÷��̾� ĳ���� ��ġ�� ��ǥ ��ġ�� �����մϴ�.
        AddBehavior(() => new BT_TargetPositionToPlayerPosition(
            SkeletonBehaviorController.KEY_PLAYERCHARACTER,
            SkeletonBehaviorController.KEY_TARGETPOSITION));

        // ��ǥ ��ġ�� �̵��մϴ�.
        AddBehavior(() => new BT_MoveTo(SkeletonBehaviorController.KEY_TARGETPOSITION, false));

        // ���
        AddBehavior(() => new BT_Wait(0.5f));
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
