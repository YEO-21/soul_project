using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 플레이어 공격 하나를 나타내기 위한 기반 클래스
/// </summary>
public abstract class PlayerAttackBase
{
    /// <summary>
    /// 공격 정보
    /// </summary>
    public PlayerAttackInfo attackInfo { get; private set; }

    /// <summary>
    /// 플레이어 공격 객체를 생성하여 반환합니다.
    /// </summary>
    /// <param name="const_PlayerAttackInfo">플레이어 공격 정보를 전달합니다.</param>
    /// <returns>플레이어 공격 객체를 생성/초기화하여 반환합니다.</returns>
    public static PlayerAttackBase GetPlayerAttack(in PlayerAttackInfo const_PlayerAttackInfo)
    {
        string attackCode = const_PlayerAttackInfo.m_PlayerAttackCode;

        
        switch (attackCode)
        {
            // 첫번째 기본 공격 객체를 반환합니다.
            case Constants.PLAYER_ATTACK_NORMAL1ST:
                return new PlayerAttack_Normal1st().Initialize(const_PlayerAttackInfo); 

            // 두번째 기본 공격 객체를 반환합니다.
            case Constants.PLAYER_ATTACK_NORMAL2ND:
                return new PlayerAttack_Normal2nd().Initialize(const_PlayerAttackInfo);

            // 세번째 기본 공격 객체를 반환합니다.
            case Constants.PLAYER_ATTACK_NORMAL3RD:
                return new PlayerAttack_Normal3rd().Initialize(const_PlayerAttackInfo);
        }

        return null;

    }
    public virtual PlayerAttackBase Initialize(in PlayerAttackInfo attackInfo)
    {
        this.attackInfo = attackInfo;
        return this;
    }

    /// <summary>
    /// 연계 가능한 공격 코드로 변환합니다.
    /// </summary>
    /// <param name="nextAttackCode">이 공격 객체와 비교될 공격 코드가 전달됩니다.</param>
    /// <returns>연계 가능한 공격 코드로 변환후 반환합니다.</returns>
    public virtual string ConvertToLinkableAttackCode(string attackCode) => null;

}
