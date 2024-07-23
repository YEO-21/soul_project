using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(
    fileName = "EnemyInfo",
    menuName = "ScriptableObject/EnemyInfo",
    order = int.MinValue)]
public sealed class EnemyInfoScriptableObject : ScriptableObject
{
    /// <summary>
    /// �� �������� ��Ÿ���ϴ�.
    /// </summary>
    public List<EnemyInfo> m_EnemyInfos;

    /// <summary>
    /// �ڵ�� ��ġ�ϴ� �� ������ ����ϴ�.
    /// </summary>
    /// <param name="enemyCode">�� �ڵ带 �����մϴ�.</param>
    /// <param name="out_EnemyInfo">�� ������ ��ȯ���� ������ �����մϴ�.</param>
    /// <returns></returns>
    public bool TryGetEnemyInfo(string enemyCode, out EnemyInfo out_EnemyInfo)
    {
        // enemyCode �� ��ġ�ϴ� �ڵ带 ���� ������ ã���ϴ�.
        EnemyInfo findedEnemyInfo = m_EnemyInfos.Find(
            (elem) => elem.m_EnemyCode == enemyCode);

        out_EnemyInfo = findedEnemyInfo;

        return out_EnemyInfo != null;
    }
}

[System.Serializable]
public sealed class EnemyInfo
{
    [Header("# �� �ڵ�")]
    public string m_EnemyCode;

    [Header("# �� �̸�")]
    public string m_Name;

    [Header("# �ִ� ü��")]
    public float m_MaxHp;

    [Header("# ���ݷ�")]
    public float m_Atk;

}
