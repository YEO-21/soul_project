using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
    private GameScenePlayerState _PlayerState;

    /// <summary>
    /// 퀵 슬롯에 표시되는 아이템 정보를 나타냅니다.
    /// </summary>
    public List<InventoryItemInfo> _QuickSlotItemInfos = new();

    /// <summary>
    /// 퀵 슬롯 아이템 갱신됨 이벤트
    /// 추후 퀵 슬롯 개수를 증설하는 경우
    /// 매개 변수 목록에 퀵 슬롯 인덱스를 전달할 수 있는 매개 변수를 추가해야 합니다.
    /// </summary>
    public event System.Action<InventoryItemInfo> onQuickSlotItemUpdated;

    public void Initialize(GameScenePlayerController playerController)
    {
        _PlayerState = playerController.playerState as GameScenePlayerState;
    }

    public void SetQuickSlotItem(string itemCode)
    {
        // itemCode 와 일치하는 인벤토리 아이템 정보를 얻습니다.
        InventoryItemInfo itemInfo = _PlayerState.GetItemInfo(itemCode);

        // 아이템 정보를 인벤토리에서 찾지 못한 경우
        if(itemInfo == null)
        {
            Debug.Log($"인벤토리에 {itemCode}와 일치하는 아이템이 존재하지 않습니다");
            return;
        }

        // 퀵 슬롯 리스트에 추가합니다.
        //_QuickSlotItemInfos.Add(itemInfo);

        // 퀵 슬롯 인덱스
        // 추후 퀵 슬롯 개수를 증설하는 경우
        // 이 지역 변수를 매개 변수로 추가하여 사용합니다.
        int quickSlotIndex = 0;

        // 퀵 슬롯에 아이템이 존재하지 않는다면
        if(_QuickSlotItemInfos.Count < quickSlotIndex + 1)
            _QuickSlotItemInfos.Add(itemInfo);

        // 이미 해당하는 퀵 슬롯에 아이템이 존재한다면
        else _QuickSlotItemInfos[quickSlotIndex] = itemInfo;

        // 퀵 슬롯 아이템 갱신됨 이벤트
        onQuickSlotItemUpdated?.Invoke(itemInfo);

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



    }

}
