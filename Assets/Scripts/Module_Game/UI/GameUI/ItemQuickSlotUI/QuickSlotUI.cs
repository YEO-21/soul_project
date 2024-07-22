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


   // ������ ������ �����մϴ�.
   public void OnQuickSlotItemChanged(InventoryItemInfo itemInfo)
    {
        // OnQuickSlotItemChanged() �޼��带 �����ϰ� ��򰡿��� ȣ�����ּ���.
        // m_CountText �� ���� itemInfo�� Count�� ����
        // �� ���� ������ ������ 0�� ���, ������ �̹����� �˰� ǥ�õǵ��� �غ�����.

        int itemCount = itemInfo.itemCount;

        // ������ ���� �ؽ�Ʈ ����
        m_CountText.text = itemCount.ToString();

        m_ItemImage.color = (itemCount == 0) ? Color.black : Color.white;



    }


}
