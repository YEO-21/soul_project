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
    /// ���� ����
    /// </summary>
    public PlayerAttackInfo attackInfo { get; private set; }

    /// <summary>
    /// �÷��̾� ���� ��ü�� �����Ͽ� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="const_PlayerAttackInfo">�÷��̾� ���� ������ �����մϴ�.</param>
    /// <returns>�÷��̾� ���� ��ü�� ����/�ʱ�ȭ�Ͽ� ��ȯ�մϴ�.</returns>
    public static PlayerAttackBase GetPlayerAttack(in PlayerAttackInfo const_PlayerAttackInfo)
    {
        string attackCode = const_PlayerAttackInfo.m_PlayerAttackCode;

        
        switch (attackCode)
        {
            // ù��° �⺻ ���� ��ü�� ��ȯ�մϴ�.
            case Constants.PLAYER_ATTACK_NORMAL1ST:
                return new PlayerAttack_Normal1st().Initialize(const_PlayerAttackInfo); 

            // �ι�° �⺻ ���� ��ü�� ��ȯ�մϴ�.
            case Constants.PLAYER_ATTACK_NORMAL2ND:
                return new PlayerAttack_Normal2nd().Initialize(const_PlayerAttackInfo);

            // ����° �⺻ ���� ��ü�� ��ȯ�մϴ�.
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
    /// ���� ������ ���� �ڵ�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="nextAttackCode">�� ���� ��ü�� �񱳵� ���� �ڵ尡 ���޵˴ϴ�.</param>
    /// <returns>���� ������ ���� �ڵ�� ��ȯ�� ��ȯ�մϴ�.</returns>
    public virtual string ConvertToLinkableAttackCode(string attackCode) => null;

}
