using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �κ��丮�� ������ ������ ��Ÿ���� ���� Ŭ�����Դϴ�.
/// </summary>
public class InventoryItemInfo
{
    public string itemCode;
    public int itemCount;


    public InventoryItemInfo() { }  

    public InventoryItemInfo(string itemCode, int itemCount = 1)
    {
        this.itemCode = itemCode;
        this.itemCount = itemCount;
    }
}
