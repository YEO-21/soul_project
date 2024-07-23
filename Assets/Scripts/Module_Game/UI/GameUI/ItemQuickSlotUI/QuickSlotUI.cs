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


   // 아이템 정보를 갱신합니다.
   public void OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        // OnQuickSlotItemChanged() 메서드를 적절하게 어딘가에서 호출해주세요.
        // m_CountText 의 값을 itemInfo의 Count로 설정
        // 퀵 슬롯 아이템 개수가 0인 경우, 아이템 이미지가 검게 표시되도록 해보세요.

        int itemCount = itemInfo.itemCount;

        // 아이템 개수 텍스트 설정
        m_CountText.text = itemCount.ToString();

        m_ItemImage.color = (itemCount == 0) ? Color.black : Color.white;



    }


}
