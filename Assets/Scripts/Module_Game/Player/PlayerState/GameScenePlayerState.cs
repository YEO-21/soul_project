using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 맵에서 플레이어 상태를 나타내기 위한 클래스입니다.
/// </summary>
public sealed class GameScenePlayerState : PlayerStateBase
{
    public float maxHp { get; private set; }
    public float hp { get; private set; }

    /// <summary>
    /// 인벤토리 아이템 정보를 나타냅니다.
    /// </summary>
    public List<InventoryItemInfo> inventoryItemInfos { get; private set; }

    /// <summary>
    /// Hp 변경됨 이벤트
    /// </summary>
    public event System.Action<float /*maxHp*/, float /*hp*/> onHpChanged;

    public GameScenePlayerState(float initialHp)
    {
        hp = maxHp = initialHp;
        inventoryItemInfos = new();
    }



    /// <summary>
    /// Hp 수치를 설정합니다.
    /// </summary>
    /// <param name="newHp"></param>
    public void SetHp(float newHp)
    {
        hp = newHp;
        if (hp > maxHp) hp = maxHp;

        // 체력 변경됨 이벤트 발생
        onHpChanged?.Invoke(maxHp, hp);
    }

    /// <summary>
    /// 인벤토리에 추가된 아이템 정보를 얻습니다.
    /// </summary>
    /// <param name="itemCode">찾고자 하는 아이템 코드를 전달합니다.</param>
    /// <returns></returns>
    public InventoryItemInfo GetItemInfo(string itemCode)
        => inventoryItemInfos.Find(itemInfo => itemInfo.itemCode == itemCode);

    /// <summary>
    /// 아이템 정보를 설정합니다.
    /// </summary>
    /// <param name="newItemInfo">아이템 정보를 전달합니다.</param>
    public void SetItemInfo(InventoryItemInfo newItemInfo)
    {
        // newItemInfo 의 아이템 코드와 일치하는 요소 인덱스를 찾습니다.
        int index = 
            inventoryItemInfos.FindIndex(itemInfo => itemInfo.itemCode == newItemInfo.itemCode);

        // 아이템을 찾지 못한 경우 요소 추가
        if(index == -1)
            inventoryItemInfos.Add(newItemInfo);

        // 아이템을 찾은 경우 아이템 설정
        else inventoryItemInfos[index] = newItemInfo;

        foreach(var info in inventoryItemInfos)
        {
            Debug.Log($"추가된 아이템 : {info.itemCode}");
        }
    }
}
