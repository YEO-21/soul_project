
using System.Collections;
using UnityEngine;

public class BT_TargetPositionToPlayerPosition : RunnableBehavior
{
    private string _PlayerCharacterKey;
    private string _TargetCharacterKey;


    public BT_TargetPositionToPlayerPosition(
        string playerCharacterKey,
        string targetPositionKey)
    {
        _PlayerCharacterKey = playerCharacterKey;
        _TargetCharacterKey = targetPositionKey;
    }


    public override IEnumerator OnBehaivorStarted()
    {
        PlayerCharacter playerCharacter =
            behaviorController.GetKey<PlayerCharacter>(_PlayerCharacterKey);

        if(!playerCharacter) 
        { 
            isSucceeded = false;
            yield break;
        }

        Vector3 targetPosition = playerCharacter.transform.position;
        behaviorController.SetKey(_TargetCharacterKey, targetPosition);
        isSucceeded = true;
    }
}
