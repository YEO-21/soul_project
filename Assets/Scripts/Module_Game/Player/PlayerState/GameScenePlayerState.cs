using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �ʿ��� �÷��̾� ���¸� ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public sealed class GameScenePlayerState : PlayerStateBase
{
    public float maxHp { get; private set; }
    public float hp { get; private set; }

    /// <summary>
    /// �κ��丮 ������ ������ ��Ÿ���ϴ�.
    /// </summary>
    public List<InventoryItemInfo> inventoryItemInfos { get; private set; }

    /// <summary>
    /// Hp ����� �̺�Ʈ
    /// </summary>
    public event System.Action<float /*maxHp*/, float /*hp*/> onHpChanged;

    public GameScenePlayerState(float initialHp)
    {
        hp = maxHp = initialHp;
        inventoryItemInfos = new();
    }



    /// <summary>
    /// Hp ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="newHp"></param>
    public void SetHp(float newHp)
    {
        hp = newHp;
        if (hp > maxHp) hp = maxHp;

        // ü�� ����� �̺�Ʈ �߻�
        onHpChanged?.Invoke(maxHp, hp);
    }

    /// <summary>
    /// �κ��丮�� �߰��� ������ ������ ����ϴ�.
    /// </summary>
    /// <param name="itemCode">ã���� �ϴ� ������ �ڵ带 �����մϴ�.</param>
    /// <returns></returns>
    public InventoryItemInfo GetItemInfo(string itemCode)
        => inventoryItemInfos.Find(itemInfo => itemInfo.itemCode == itemCode);

    /// <summary>
    /// ������ ������ �����մϴ�.
    /// </summary>
    /// <param name="newItemInfo">������ ������ �����մϴ�.</param>
    public void SetItemInfo(InventoryItemInfo newItemInfo)
    {
        // newItemInfo �� ������ �ڵ�� ��ġ�ϴ� ��� �ε����� ã���ϴ�.
        int index = 
            inventoryItemInfos.FindIndex(itemInfo => itemInfo.itemCode == newItemInfo.itemCode);

        // �������� ã�� ���� ��� ��� �߰�
        if(index == -1)
            inventoryItemInfos.Add(newItemInfo);

        // �������� ã�� ��� ������ ����
        else inventoryItemInfos[index] = newItemInfo;

        foreach(var info in inventoryItemInfos)
        {
            Debug.Log($"�߰��� ������ : {info.itemCode}");
        }
    }
}