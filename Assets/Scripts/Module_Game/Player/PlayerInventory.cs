using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
    private GameScenePlayerState _PlayerState;

    /// <summary>
    /// �� ���Կ� ǥ�õǴ� ������ ������ ��Ÿ���ϴ�.
    /// </summary>
    public List<InventoryItemInfo> _QuickSlotItemInfos = new();


    /// <summary>
    /// �� ���� ������ ���ŵ� �̺�Ʈ
    /// ���� �� ���� ������ �����ϴ� ���
    /// �Ű� ���� ��Ͽ� �� ���� �ε����� ������ �� �ִ� �Ű� ������ �߰��ؾ� �մϴ�.
    /// </summary>
    public event System.Action<InventoryItemInfo> onQuickSlotItemUpdated;

    public void Initialize(GameScenePlayerController playerController)
    {
        _PlayerState = playerController.playerState as GameScenePlayerState;
    }

    public void SetQuickSlotItem(string itemCode)
    {
        // itemCode �� ��ġ�ϴ� �κ��丮 ������ ������ ����ϴ�.
        InventoryItemInfo itemInfo = _PlayerState.GetItemInfo(itemCode);

        // ������ ������ �κ��丮���� ã�� ���� ���
        if(itemInfo == null)
        {
            Debug.Log($"�κ��丮�� {itemCode}�� ��ġ�ϴ� �������� �������� �ʽ��ϴ�");
            return;
        }

        // �� ���� ����Ʈ�� �߰��մϴ�.
        //_QuickSlotItemInfos.Add(itemInfo);

        // �� ���� �ε���
        // ���� �� ���� ������ �����ϴ� ���
        // �� ���� ������ �Ű� ������ �߰��Ͽ� ����մϴ�.
        int quickSlotIndex = 0;

        // �� ���Կ� �������� �������� �ʴ´ٸ�
        if(_QuickSlotItemInfos.Count < quickSlotIndex + 1)
            _QuickSlotItemInfos.Add(itemInfo);

        // �̹� �ش��ϴ� �� ���Կ� �������� �����Ѵٸ�
        else _QuickSlotItemInfos[quickSlotIndex] = itemInfo;

        // �� ���� ������ ���ŵ� �̺�Ʈ
        onQuickSlotItemUpdated?.Invoke(itemInfo);

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
        // itemCode �� ��ġ�ϴ� ������ ������ ����ϴ�.
        InventoryItemInfo itemInfo = _PlayerState.GetItemInfo(itemCode);

        // ������ ������ ������ ���
        if(itemInfo.itemCount <1)
        {
            Debug.Log("������ ������ �����մϴ�.");
            return;
        }


        // ������ ����
        --itemInfo.itemCount;

        // ������ ������ ������ ����
        _PlayerState.SetItemInfo(itemInfo);

        // ����� ������ ������ UI �� �ݿ�
        //SetQuickSlotItem(itemCode);
        onQuickSlotItemUpdated?.Invoke(itemInfo);

        // ������ ����
        OnItemUsed(itemInfo);

    }

    /// <summary>
    /// �������� ���Ǿ��� ��� ȣ��˴ϴ�.
    /// </summary>
    /// <param name="usedItem">���� ������ ������ ���޵˴ϴ�.</param>
    private void OnItemUsed(InventoryItemInfo usedItem)
    {
        switch ((usedItem.itemCode))
        {
            // Hp ȸ�� ������
            case "000001":
                {
                    // ���� ü��
                    float currentHp = _PlayerState.hp;
                    currentHp += 10.0f;
                    _PlayerState.SetHp(currentHp);
                }
                break;

        }


    }

}
