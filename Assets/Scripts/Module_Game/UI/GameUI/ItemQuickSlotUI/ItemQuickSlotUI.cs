using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀵 슬롯 UI 전체를 관리하기 위한 컴포넌트
/// </summary>
public sealed class ItemQuickSlotUI : MonoBehaviour
{
    [Header("# 퀵 슬롯")]
    public List<QuickSlotUI> m_QuickSlotUI;

    public void InitializeUI(GameScenePlayerController playerController)
    {
        // 퀵 슬롯 아이템 변경됨 콜백 등록
        playerController.inventory.onQuickSlotItemUpdated += CALLBACK_OnQuickSlotItemChanged;
   
    }
   
    private void CALLBACK_OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        Debug.Log($"퀵슬롯에 등록된 아이템 코드 : {itemInfo.itemCode}");
        Debug.Log($"퀵슬롯에 등록된 아이템 개수 : {itemInfo.itemCount}");
    }

}
