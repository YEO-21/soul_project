using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "PlayerAttackInfo",
    menuName = "ScriptableObject/PlayerAttackInfo",
    order = int.MinValue)]
public sealed class PlayerAttackInfoScriptableObject : ScriptableObject
{
    /// <summary>
    /// 플레이어 공격 정보들을 나타냅니다.
    /// </summary>
    public List<PlayerAttackInfo> m_PlayerAttackInfos;
    

    /// <summary>
    /// 코드와 일치하는 플레이어 공격 정보를 얻습니다.
    /// </summary>
    /// <param name="attackCode">공격 코드를 전달합니다.</param>
    /// <param name="out_PlayerAttackInfo">공격 정보를 반환받을 변수를 전달합니다.</param>
    /// <returns>공격 정보 탐색 성공 여부를 반환합니다.</returns>
    public bool TryGetPlayerAttackInfo(
        string attackCode,
        out PlayerAttackInfo out_PlayerAttackInfo)
    {
        // attackCode 와 일치하는 공격 정보를 탐색합니다.
        PlayerAttackInfo findedInfo = m_PlayerAttackInfos.Find(
            (PlayerAttackInfo attackInfo) => attackInfo.m_PlayerAttackCode == attackCode);

        if (findedInfo == null) 
        {
            out_PlayerAttackInfo = null;
            return false;
        }

        out_PlayerAttackInfo = findedInfo;
        return true;
    }


}

/// <summary>
/// 플레이어 공격 정보를 나타냅니다.
/// </summary>
[Serializable]
public sealed class PlayerAttackInfo
{
    [Header("# 공격 이름")]
    public string m_AttackName;

    [Header("# 공격 코드")]
    public string m_PlayerAttackCode;

    [Header("# 중복 피해 허용 여부")]
    public bool m_AlllowDuplicateDamage;

    [Header("# 피해량")]
    public float m_Damage;


    /// <summary>
    /// 정수 형태의 공격 코드를 나타냅니다.
    /// </summary>
    private int _IntAttackCode = -1;

    public int intAttackCode =>(_IntAttackCode <0) ? 
        (_IntAttackCode = int.Parse(m_PlayerAttackCode)) : _IntAttackCode;
}
