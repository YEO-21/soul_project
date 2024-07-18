using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
    /// <summary>
    /// �κ��丮 ������ ������ ��Ÿ���ϴ�.
    /// </summary>
    private List<InventoryItemInfo> _InventoryItemInfos = new();

    /// <summary>
    /// �� ���Կ� ǥ�õǴ� ������ ������ ��Ÿ���ϴ�.
    /// </summary>
    public List<InventoryItemInfo> _QuickSlotItemInfos = new();

    public void Initialize(GameScenePlayerController playerController)
    {
    }

    /// <summary>
    /// �� ���Կ� ǥ�õǴ� �������� ����մϴ�.
    /// </summary>
    /// <param name="index">�� ���� �ε����� �����մϴ�.</param>
    public void UseItemFromQuickSlot(int index)
    {
        // index �� �ش��ϴ� ������ ������ ����ϴ�.
        InventoryItemInfo quickSlotItemInfo = _QuickSlotItemInfos[index];

        // ������ ���
        UseItem(quickSlotItemInfo.itemCode);
    }

    public void UseItem(string itemCode)
    {
        // itemCode �� �ش��ϴ� ������ ������ ����ϴ�.
        InventoryItemInfo invenItemInfo = _InventoryItemInfos.Find(
            invenItemInfo => invenItemInfo.itemCode == itemCode);

        // itemCode �� �ش��ϴ� �������� �κ��丮���� ã�� ���� ��� �Լ� ȣ�� ����
        if (invenItemInfo == null) return;

        // ������ ������ ������ ��� �Լ� ȣ�� ����.
        if (invenItemInfo.itemCount == 0) return;

        // ������ ������ ���ҽ�ŵ�ϴ�.
        --invenItemInfo.itemCount;


    }

}
