using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerDamagedStateMachineBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        PlayerCharacterAnimController animController = animator.GetComponent<PlayerCharacterAnimController>();
        animController.OnHitAnimationFinished();
    }

}
