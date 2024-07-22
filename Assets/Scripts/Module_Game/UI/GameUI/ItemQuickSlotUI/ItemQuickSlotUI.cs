using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �� ���� UI ��ü�� �����ϱ� ���� ������Ʈ
/// </summary>
public sealed class ItemQuickSlotUI : MonoBehaviour
{
    [Header("# �� ����")]
    public List<QuickSlotUI> m_QuickSlotUI;

    [Header("# �� ���� �̹���")]
    public Image m_QuickSlotImage;

   

    public event System.Action onQuickSlotEmpty;

    public void InitializeUI(GameScenePlayerController playerController)
    {
        // �� ���� ������ ����� �ݹ� ���
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
