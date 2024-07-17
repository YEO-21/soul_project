using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(
    fileName = "EnemyInfo",
    menuName = "ScriptableObject/EnemyInfo",
    order = int.MinValue)]
public sealed class EnemyInfoScriptableObject : ScriptableObject
{
    /// <summary>
    /// 적 정보들을 나타냅니다.
    /// </summary>
    public List<EnemyInfo> m_EnemyInfos;

    /// <summary>
    /// 코드와 일치하는 적 정보를 얻습니다.
    /// </summary>
    /// <param name="enemyCode">적 코드를 전달합니다.</param>
    /// <param name="out_EnemyInfo">적 정보를 반환받을 변수를 전달합니다.</param>
    /// <returns></returns>
    public bool TryGetEnemyInfo(string enemyCode, out EnemyInfo out_EnemyInfo)
    {
        // enemyCode 와 일치하는 코드를 가진 정보를 찾습니다.
        EnemyInfo findedEnemyInfo = m_EnemyInfos.Find(
            (elem) => elem.m_EnemyCode == enemyCode);

        out_EnemyInfo = findedEnemyInfo;

        return out_EnemyInfo != null;
    }
}

[System.Serializable]
public sealed class EnemyInfo
{
    [Header("# 적 코드")]
    public string m_EnemyCode;

    [Header("# 적 이름")]
    public string m_Name;

    [Header("# 최대 체력")]
    public float m_MaxHp;

    [Header("# 공격력")]
    public float m_Atk;

}
