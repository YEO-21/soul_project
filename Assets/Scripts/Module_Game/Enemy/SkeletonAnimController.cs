using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonAnimController : AnimController
{
    private const string PARAM_SPEED = "_Speed";

   public void Initialize(EnemySkeleton skeleton)
    {
        skeleton.onMoveSpeedChanged += CALLBACK_OnMoveSpeedChanged;
    }

    private void CALLBACK_OnMoveSpeedChanged(float speed)
    {
        SetParam(PARAM_SPEED, speed);
    }


}
