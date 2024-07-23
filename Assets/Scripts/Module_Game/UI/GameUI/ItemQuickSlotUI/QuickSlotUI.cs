using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    [Header("# 개수 텍스트")]
    public TMP_Text m_CountText;

    [Header("# 아이템 이미지")]
    public Image m_ItemImage;


    

    /// <summary>
    /// 아이템 정보를 갱신합니다.
    /// </summary>
    /// <param name="itemInfo"></param>
    public void OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        int itemCount = itemInfo.itemCount;

        m_CountText.text = itemCount.ToString();
        m_ItemImage.color = (itemCount == 0) ? Color.black : Color.white;
    }
}
