using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Normal2nd : PlayerAttackBase
{
    public override string ConvertToLinkableAttackCode(string attackCode)
     => (attackCode == Constants.PLAYER_ATTACK_NORMAL) ?
        Constants.PLAYER_ATTACK_NORMAL3RD : null;


}
