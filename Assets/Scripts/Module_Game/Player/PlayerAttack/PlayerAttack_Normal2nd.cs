using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerAttack_Normal2nd : PlayerAttackBase
{

    public override string ConvertToLinkableAttackCode(string attackCode)
        => (attackCode == Constants.PLAYER_ATTACKCODE_NORMAL) ?
        Constants.PLAYER_ATTACKCODE_NORMAL3RD : null;
}
