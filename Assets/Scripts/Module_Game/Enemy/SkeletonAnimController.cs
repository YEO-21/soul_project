using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkeletonAnimController : AnimController
{
    private const string PARAM_SPEED = "_Speed";
    private const string PARAM_ISDAMAGED = "_IsDamaged";
    private const string PARAM_DAMAGEDDIRECTIONZ = "_DamagedDirectionZ";

    private EnemySkeleton _Skeleton;

   public void Initialize(EnemySkeleton skeleton)
    {
        _Skeleton = skeleton;

        skeleton.onMoveSpeedChanged += CALLBACK_OnMoveSpeedChanged;
        skeleton.onHit += CALLBACK_OnDamaged;

    }

    private void CALLBACK_OnMoveSpeedChanged(float speed)
    {
        SetParam(PARAM_SPEED, speed);
    }

    private void CALLBACK_OnDamaged(DamageBase damageInstance)
    {
        
        
        // 뒤에서 공격 받았을 경우 -1
        // 앞에서 공격 받았을 경우 1

    }

}
