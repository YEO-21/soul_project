using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    [Header("# ���� �ؽ�Ʈ")]
    public TMP_Text m_CountText;

    [Header("# ������ �̹���")]
    public Image m_ItemImage;


    

    /// <summary>
    /// ������ ������ �����մϴ�.
    /// </summary>
    /// <param name="itemInfo"></param>
    public void OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        int itemCount = itemInfo.itemCount;

        m_CountText.text = itemCount.ToString();
        m_ItemImage.color = (itemCount == 0) ? Color.black : Color.white;
    }
}
