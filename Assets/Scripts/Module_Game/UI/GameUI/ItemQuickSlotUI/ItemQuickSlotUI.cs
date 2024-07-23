using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ���� UI ��ü�� �����ϱ� ���� ������Ʈ
/// </summary>
public sealed class ItemQuickSlotUI : MonoBehaviour
{
    [Header("# �� ����")]
    public List<QuickSlotUI> m_QuickSlotUI;



    public void InitializeUI(GameScenePlayerController playerController)
    {
        // �� ���� ������ ����� �ݹ� ���
        playerController.inventory.onQuickSlotItemUpdated += CALLBACK_OnQuickSlotItemChanged;
    }

    private void CALLBACK_OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        m_QuickSlotUI[0].OnQuickSlotItemChanged(itemInfo);
    }


}
