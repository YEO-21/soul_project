

public sealed class SkeletonAttackSelector : BehaviorSelector
{
    public SkeletonAttackSelector()
    {
        AddBehavior(() => new BT_SkeletonAttack(
            SkeletonBehaviorController.KEY_ISATTACKABLE));

    }

    public override bool OnInitialized(BehaviorController behaviorController)
    {
        base.OnInitialized(behaviorController);

        return behaviorController.GetKey<bool>(
            SkeletonBehaviorController.KEY_ISATTACKABLE);
    }


}