using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Normal3rd : PlayerAttackBase
{
    public override bool IsAttackAddable(string nextAttackCode) //=> false;
        => (nextAttackCode != Constants.PLAYER_ATTACK_NORMAL);
    

}
