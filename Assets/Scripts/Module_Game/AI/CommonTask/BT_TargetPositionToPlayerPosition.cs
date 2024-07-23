
using System.Collections;
using UnityEngine;

public sealed class BT_TargetPositionToPlayerPosition : RunnableBehavior
{
    private string _PlayerCharacterKey;
    private string _TargetPositionKey;

    public BT_TargetPositionToPlayerPosition(
        string playerCharacterKey,
        string targetPositionKey)
    {
        _PlayerCharacterKey = playerCharacterKey;
        _TargetPositionKey = targetPositionKey;
    }

    public override IEnumerator OnBehaviorStarted()
    {
        PlayerCharacter playerCharacter = behaviorController.GetKey<PlayerCharacter>(
            _PlayerCharacterKey);

        if (!playerCharacter)
        {
            isSucceeded = false;
            yield break;
        }

        Vector3 targetPosition = playerCharacter.transform.position;
        behaviorController.SetKey(_TargetPositionKey, targetPosition);
        isSucceeded = true;
    }
}
