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
    /// �÷��̾� ���� �������� ��Ÿ���ϴ�.
    /// </summary>
    public List<PlayerAttackInfo> m_PlayerAttackInfos;
    

    /// <summary>
    /// �ڵ�� ��ġ�ϴ� �÷��̾� ���� ������ ����ϴ�.
    /// </summary>
    /// <param name="attackCode">���� �ڵ带 �����մϴ�.</param>
    /// <param name="out_PlayerAttackInfo">���� ������ ��ȯ���� ������ �����մϴ�.</param>
    /// <returns>���� ���� Ž�� ���� ���θ� ��ȯ�մϴ�.</returns>
    public bool TryGetPlayerAttackInfo(
        string attackCode,
        out PlayerAttackInfo out_PlayerAttackInfo)
    {
        // attackCode �� ��ġ�ϴ� ���� ������ Ž���մϴ�.
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
/// �÷��̾� ���� ������ ��Ÿ���ϴ�.
/// </summary>
[Serializable]
public sealed class PlayerAttackInfo
{
    [Header("# ���� �̸�")]
    public string m_AttackName;

    [Header("# ���� �ڵ�")]
    public string m_PlayerAttackCode;

    [Header("# �ߺ� ���� ��� ����")]
    public bool m_AlllowDuplicateDamage;

    [Header("# ���ط�")]
    public float m_Damage;


    /// <summary>
    /// ���� ������ ���� �ڵ带 ��Ÿ���ϴ�.
    /// </summary>
    private int _IntAttackCode = -1;

    public int intAttackCode =>(_IntAttackCode <0) ? 
        (_IntAttackCode = int.Parse(m_PlayerAttackCode)) : _IntAttackCode;
}
