using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Äü ½½·Ô UI ÀüÃ¼¸¦ °ü¸®ÇÏ±â À§ÇÑ ÄÄÆ÷³ÍÆ®
/// </summary>
public sealed class ItemQuickSlotUI : MonoBehaviour
{
    [Header("# Äü ½½·Ô")]
    public List<QuickSlotUI> m_QuickSlotUI;

    [Header("# Äü ½½·Ô ÀÌ¹ÌÁö")]
    public Image m_QuickSlotImage;

   

    public event System.Action onQuickSlotEmpty;

    public void InitializeUI(GameScenePlayerController playerController)
    {
        // Äü ½½·Ô ¾ÆÀÌÅÛ º¯°æµÊ ÄÝ¹é µî·Ï
        playerController.inventory.onQuickSlotItemUpdated += CALLBACK_OnQuickSlotItemChanged;
   
    }
   
    private void CALLBACK_OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        #region MyStyle
        // if (m_QuickSlotUI.Count < 1) m_QuickSlotImage.color = Color.gray;

        //foreach(QuickSlotUI quickSlot in m_QuickSlotUI)
        //     quickSlot.OnQuickSlotItemChanged(itemInfo);
        #endregion

        m_QuickSlotUI[0].OnQuickSlotItemChanged(itemInfo);

    }

}
