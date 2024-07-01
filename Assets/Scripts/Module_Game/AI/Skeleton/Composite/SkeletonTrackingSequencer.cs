

using UnityEngine;

public sealed class SkeletonTrackingSequencer : BehaviorSequencer
{
    public SkeletonTrackingSequencer()
    {
        // ���� ���� ���� �˻� ���� �߰�
        AddService(() => new BS_CheckAttackableArea(
            1.0f, 0.5f, 0.6f, LayerMask.GetMask("PlayerCharacter")));

        // �÷��̾� ĳ���� ��ġ�� ��ǥ ��ġ�� �����մϴ�.
        AddBehavior(() => new BT_TargetPositionToPlayerPosition(
            SkeletonBehaviorController.KEY_PLAYERCHARACTER,
            SkeletonBehaviorController.KEY_TARGETPOSITION));

        // ��ǥ ��ġ�� �̵��մϴ�.
        AddBehavior(() => new BT_MoveTo(SkeletonBehaviorController.KEY_TARGETPOSITION, false));

        // ���
        AddBehavior(() => new BT_Wait(0.5f));
    }
}
