using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
    /// <summary>
    /// 인벤토리 아이템 정보를 나타냅니다.
    /// </summary>
    private List<InventoryItemInfo> _InventoryItemInfos = new();

    /// <summary>
    /// 퀵 슬롯에 표시되는 아이템 정보를 나타냅니다.
    /// </summary>
    public List<InventoryItemInfo> _QuickSlotItemInfos = new();

    public void Initialize(GameScenePlayerController playerController)
    {
    }

    /// <summary>
    /// 퀵 슬롯에 표시되는 아이템을 사용합니다.
    /// </summary>
    /// <param name="index">퀵 슬롯 인덱스를 전달합니다.</param>
    public void UseItemFromQuickSlot(int index)
    {
        // index 에 해당하는 아이템 정보를 얻습니다.
        InventoryItemInfo quickSlotItemInfo = _QuickSlotItemInfos[index];

        // 아이템 사용
        UseItem(quickSlotItemInfo.itemCode);
    }

    public void UseItem(string itemCode)
    {
        // itemCode 에 해당하는 아이템 정보를 얻습니다.
        InventoryItemInfo invenItemInfo = _InventoryItemInfos.Find(
            invenItemInfo => invenItemInfo.itemCode == itemCode);

        // itemCode 에 해당하는 아이템을 인벤토리에서 찾지 못한 경우 함수 호출 종료
        if (invenItemInfo == null) return;

        // 아이템 개수가 부족한 경우 함수 호출 종료.
        if (invenItemInfo.itemCount == 0) return;

        // 아이템 개수를 감소시킵니다.
        --invenItemInfo.itemCount;


    }

}
