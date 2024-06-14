using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Normal1st : PlayerAttackBase
{
    /// <summary>
    /// 기본 공격이 요청된 경우에만 두번째 기본 공격 코드를 반환합니다.
    /// </summary>
    /// <param name="attackCode"></param>
    /// <returns></returns>
    public override string ConvertToLinkableAttackCode(string attackCode)
        => (attackCode == Constants.PLAYER_ATTACK_NORMAL) ?
        Constants.PLAYER_ATTACK_NORMAL2ND : null;

}
