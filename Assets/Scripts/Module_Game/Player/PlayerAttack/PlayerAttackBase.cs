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
    ///  이미 공격된 적 객체를 기록할 리스트입니다.
    ///  중복 공격을 막기 위하여 사용됩니다.
    /// </summary>
    private List<IDamageable> _AttackedEnemies;

    /// <summary>
    /// 공격을 실행하고 있는 플레이어 캐릭터 객체를 나타냅니다.
    /// </summary>
    private PlayerCharacter _PlayerCharacter;

    /// <summary>
    /// 공격 정보
    /// </summary>
    public PlayerAttackInfo attackInfo { get; private set; }



    /// <summary>
    /// 플레이어 공격 객체를 생성하여 반환합니다.
    /// </summary>
    /// <param name="const_PlayerAttackInfo">플레이어 공격 정보를 전달합니다.</param>
    /// <returns>플레이어 공격 객체를 생성/초기화하여 반환합니다.</returns>
    public static PlayerAttackBase GetPlayerAttack(
        PlayerCharacter owner,
        in PlayerAttackInfo const_PlayerAttackInfo)
    {
        string attackCode = const_PlayerAttackInfo.m_PlayerAttackCode;

        
        switch (attackCode)
        {
            // 첫번째 기본 공격 객체를 반환합니다.
            case Constants.PLAYER_ATTACK_NORMAL1ST:
                return new PlayerAttack_Normal1st().Initialize(owner, const_PlayerAttackInfo); 

            // 두번째 기본 공격 객체를 반환합니다.
            case Constants.PLAYER_ATTACK_NORMAL2ND:
                return new PlayerAttack_Normal2nd().Initialize(owner, const_PlayerAttackInfo);

            // 세번째 기본 공격 객체를 반환합니다.
            case Constants.PLAYER_ATTACK_NORMAL3RD:
                return new PlayerAttack_Normal3rd().Initialize(owner,const_PlayerAttackInfo);
        }

        return null;

    }
    public virtual PlayerAttackBase Initialize(PlayerCharacter owner, in PlayerAttackInfo attackInfo)
    {
        _PlayerCharacter = owner;
        this.attackInfo = attackInfo;

        if(!this.attackInfo.m_AlllowDuplicateDamage)
        {
            _AttackedEnemies = new();
        }
        return this;
    }

    /// <summary>
    /// 연계 가능한 공격 코드로 변환합니다.
    /// </summary>
    /// <param name="nextAttackCode">이 공격 객체와 비교될 공격 코드가 전달됩니다.</param>
    /// <returns>연계 가능한 공격 코드로 변환후 반환합니다.</returns>
    public virtual string ConvertToLinkableAttackCode(string attackCode) => null;

    /// <summary>
    /// 공격 추각 가능 여부를 반환합니다.
    /// </summary>
    /// <param name="nextAttackCode">다음 공격 코드가 전달됩니다.</param>
    /// <returns></returns>
    public virtual bool IsAttackAddable(string nextAttackCode) => true;

    /// <summary>
    /// 피해를 입을 수 있는 객체를 감지한 경우 호출됩니다.
    /// </summary>
    /// <param name="to"></param>
    public virtual void OnDamageableObjectDetected(IDamageable to)
    {
        // 중복 공격을 허용하지 않는 경우
        if(!attackInfo.m_AlllowDuplicateDamage)
        {
            // 이미 공격 처리된 적인지 확인하고, 공격 처리된 적이라면 추가 피해를 주지 않습니다.
            if (_AttackedEnemies.Contains(to)) return;

            // 아직 공격처리되지 않은 적인 경우, 리스트에 추가하여 다음 공격을 진행하지 않도록 합니다.
            else _AttackedEnemies.Add(to);
        }


        // 감지된 객체에게 피해를 입힙니다.
        DamageBase.Hit(to, new SampleDamage(
            _PlayerCharacter.transform,
            attackInfo.m_Damage,
            attackInfo.m_IsCriticalDamage));

        

    }

}


public class SampleDamage : DamageBase
{
    public SampleDamage(Transform from, float damage, bool isCriticalDamage)
        : base(from, damage, isCriticalDamage)
    {
    }
}

