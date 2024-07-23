using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameUIPanel : MonoBehaviour
{
    private PlayerStateUI _PlayerStateUI;
    private TargetEnemyUI _TargetEnemyUI;
    private ItemQuickSlotUI _ItemQuickSlotUI;

    public PlayerStateUI playerStateUI => _PlayerStateUI ?? 
        (_PlayerStateUI = GetComponentInChildren<PlayerStateUI>());
    public TargetEnemyUI targetEnemyUI => _TargetEnemyUI ??
        (_TargetEnemyUI = GetComponentInChildren<TargetEnemyUI>());

    public ItemQuickSlotUI itemQuickSlotUI => _ItemQuickSlotUI ??
        (_ItemQuickSlotUI = GetComponentInChildren<ItemQuickSlotUI>());

    /// <summary>
    /// UI �ʱ�ȭ
    /// </summary>
    /// <param name="playerController"></param>
    public void InitializeUI(GameScenePlayerController playerController)
    {
        playerStateUI.InitializeUI(playerController);

        // ��ǥ �� ǥ�� UI �ʱ�ȭ
        targetEnemyUI.InitializeUI(playerController);

        itemQuickSlotUI.InitializeUI(playerController);
    }

}
