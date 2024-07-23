using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Äü ½½·Ô UI ÀüÃ¼¸¦ °ü¸®ÇÏ±â À§ÇÑ ÄÄÆ÷³ÍÆ®
/// </summary>
public sealed class ItemQuickSlotUI : MonoBehaviour
{
    [Header("# Äü ½½·Ô")]
    public List<QuickSlotUI> m_QuickSlotUI;



    public void InitializeUI(GameScenePlayerController playerController)
    {
        // Äü ½½·Ô ¾ÆÀÌÅÛ º¯°æµÊ ÄÝ¹é µî·Ï
        playerController.inventory.onQuickSlotItemUpdated += CALLBACK_OnQuickSlotItemChanged;
    }

    private void CALLBACK_OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        m_QuickSlotUI[0].OnQuickSlotItemChanged(itemInfo);
    }


}
