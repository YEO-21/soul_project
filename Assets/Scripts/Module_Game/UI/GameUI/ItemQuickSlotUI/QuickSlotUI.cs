using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour
{

    [Header("# ")]
    public TMP_Text m_CountText;




   // ������ ������ �����մϴ�.
   public void OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        // OnQuickSlotItemChanged() �޼��带 �����ϰ� ��򰡿��� ȣ�����ּ���.
        // m_CountText �� ���� itemInfo�� Count�� ����
        // �� ���� ������ ������ 0�� ���, ������ �̹����� �˰� ǥ�õǵ��� �غ�����.


    }


}
