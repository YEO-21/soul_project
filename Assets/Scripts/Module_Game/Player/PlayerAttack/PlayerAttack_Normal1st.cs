using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Normal1st : PlayerAttackBase
{
    /// <summary>
    /// �⺻ ������ ��û�� ��쿡�� �ι�° �⺻ ���� �ڵ带 ��ȯ�մϴ�.
    /// </summary>
    /// <param name="attackCode"></param>
    /// <returns></returns>
    public override string ConvertToLinkableAttackCode(string attackCode)
        => (attackCode == Constants.PLAYER_ATTACK_NORMAL) ?
        Constants.PLAYER_ATTACK_NORMAL2ND : null;

}
