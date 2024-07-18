using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(
    fileName = "ItemInfo",
    menuName = "ScriptableObject/ItemInfo",
    order = int.MinValue)]
public sealed class ItemInfoScriptableObject : ScriptableObject
{
    [Header("# 아이템 정보")]
    public List<ItemInfo> m_ItemInfos;

    /// <summary>
    /// 아이템 코드를 이용하여 아이템 정보를 얻습니다.
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public ItemInfo GetItemInfo(string itemCode)
        => m_ItemInfos.Find(item => itemCode == item.m_ItemCode);
    



}

/// <summary>
/// 아이템 정보 하나를 나타내기 위한 클래스입니다.
/// </summary>
[System.Serializable]
public sealed class ItemInfo
{
    [Header("# 아이템 이름")]
    public string m_ItemName;

    [Header("# 아이템 코드")]
    public string m_ItemCode;
}

