using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour
{

    [Header("# ")]
    public TMP_Text m_CountText;




   // 아이템 정보를 갱신합니다.
   public void OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        // OnQuickSlotItemChanged() 메서드를 적절하게 어딘가에서 호출해주세요.
        // m_CountText 의 값을 itemInfo의 Count로 설정
        // 퀵 슬롯 아이템 개수가 0인 경우, 아이템 이미지가 검게 표시되도록 해보세요.


    }


}
