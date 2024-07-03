

using UnityEngine;

public sealed class SkeletonTrackingSequencer : BehaviorSequencer
{
    public SkeletonTrackingSequencer()
    {
        Debug.Log("Tracking");

        // 공격 가능 영역 검사 서비스 추가
        AddService(() => new BS_CheckAttackableArea(
            1.0f, 0.5f, 0.6f, LayerMask.GetMask("PlayerCharacter"),
            SkeletonBehaviorController.KEY_ISATTACKABLE));

        // 플레이어 캐릭터 위치를 목표 위치로 설정합니다.
        AddBehavior(() => new BT_TargetPositionToPlayerPosition(
            SkeletonBehaviorController.KEY_PLAYERCHARACTER,
            SkeletonBehaviorController.KEY_TARGETPOSITION));

        // 목표 위치로 이동합니다.
        AddBehavior(() => new BT_MoveTo(SkeletonBehaviorController.KEY_TARGETPOSITION, false));

        // 대기
        AddBehavior(() => new BT_Wait(0.5f));
    }

    public override bool OnInitialized(BehaviorController behaviorController)
    {
        base.OnInitialized(behaviorController);

        return !behaviorController.GetKey<bool>(
            SkeletonBehaviorController.KEY_ISATTACKABLE);
    }
}
