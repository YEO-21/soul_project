using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 인벤토리의 아이템 정보를 나타내기 위한 클래스입니다.
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
