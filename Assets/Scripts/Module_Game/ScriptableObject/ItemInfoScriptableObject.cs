using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(
    fileName = "ItemInfo",
    menuName = "ScriptableObject/ItemInfo",
    order = int.MinValue)]
public sealed class ItemInfoScriptableObject : ScriptableObject
{
    [Header("# ������ ����")]
    public List<ItemInfo> m_ItemInfos;

    /// <summary>
    /// ������ �ڵ带 �̿��Ͽ� ������ ������ ����ϴ�.
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public ItemInfo GetItemInfo(string itemCode)
        => m_ItemInfos.Find(item => itemCode == item.m_ItemCode);
    



}

/// <summary>
/// ������ ���� �ϳ��� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
[System.Serializable]
public sealed class ItemInfo
{
    [Header("# ������ �̸�")]
    public string m_ItemName;

    [Header("# ������ �ڵ�")]
    public string m_ItemCode;
}

