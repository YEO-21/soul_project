using GameModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �÷��̾� ���� �ϳ��� ��Ÿ���� ���� ��� Ŭ����
/// </summary>
public abstract class PlayerAttackBase
{
    /// <summary>
    ///  �̹� ���ݵ� �� ��ü�� ����� ����Ʈ�Դϴ�.
    ///  �ߺ� ������ ���� ���Ͽ� ���˴ϴ�.
    /// </summary>
    private List<IDamageable> _AttackedEnemies;

    /// <summary>
    /// ������ �����ϰ� �ִ� �÷��̾� ĳ���� ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    private PlayerCharacter _PlayerCharacter;

    /// <summary>
    /// ���� ����
    /// </summary>
    public PlayerAttackInfo attackInfo { get; private set; }



    /// <summary>
    /// �÷��̾� ���� ��ü�� �����Ͽ� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="const_PlayerAttackInfo">�÷��̾� ���� ������ �����մϴ�.</param>
    /// <returns>�÷��̾� ���� ��ü�� ����/�ʱ�ȭ�Ͽ� ��ȯ�մϴ�.</returns>
    public static PlayerAttackBase GetPlayerAttack(
        PlayerCharacter owner,
        in PlayerAttackInfo const_PlayerAttackInfo)
    {
        string attackCode = const_PlayerAttackInfo.m_PlayerAttackCode;

        
        switch (attackCode)
        {
            // ù��° �⺻ ���� ��ü�� ��ȯ�մϴ�.
            case Constants.PLAYER_ATTACK_NORMAL1ST:
                return new PlayerAttack_Normal1st().Initialize(owner, const_PlayerAttackInfo); 

            // �ι�° �⺻ ���� ��ü�� ��ȯ�մϴ�.
            case Constants.PLAYER_ATTACK_NORMAL2ND:
                return new PlayerAttack_Normal2nd().Initialize(owner, const_PlayerAttackInfo);

            // ����° �⺻ ���� ��ü�� ��ȯ�մϴ�.
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
    /// ���� ������ ���� �ڵ�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="nextAttackCode">�� ���� ��ü�� �񱳵� ���� �ڵ尡 ���޵˴ϴ�.</param>
    /// <returns>���� ������ ���� �ڵ�� ��ȯ�� ��ȯ�մϴ�.</returns>
    public virtual string ConvertToLinkableAttackCode(string attackCode) => null;

    /// <summary>
    /// ���� �߰� ���� ���θ� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="nextAttackCode">���� ���� �ڵ尡 ���޵˴ϴ�.</param>
    /// <returns></returns>
    public virtual bool IsAttackAddable(string nextAttackCode) => true;

    /// <summary>
    /// ���ظ� ���� �� �ִ� ��ü�� ������ ��� ȣ��˴ϴ�.
    /// </summary>
    /// <param name="to"></param>
    public virtual void OnDamageableObjectDetected(IDamageable to)
    {
        // �ߺ� ������ ������� �ʴ� ���
        if(!attackInfo.m_AlllowDuplicateDamage)
        {
            // �̹� ���� ó���� ������ Ȯ���ϰ�, ���� ó���� ���̶�� �߰� ���ظ� ���� �ʽ��ϴ�.
            if (_AttackedEnemies.Contains(to)) return;

            // ���� ����ó������ ���� ���� ���, ����Ʈ�� �߰��Ͽ� ���� ������ �������� �ʵ��� �մϴ�.
            else _AttackedEnemies.Add(to);
        }


        // ������ ��ü���� ���ظ� �����ϴ�.
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

