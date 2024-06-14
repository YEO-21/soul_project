using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackStateMachineBehaviour : StateMachineBehaviour
{
    private const string PARAM_ISATTACKING = "_IsAttacking";

    public override void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.SetBool(PARAM_ISATTACKING, true);
    }




}
