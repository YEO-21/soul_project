using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameUIPanel : MonoBehaviour
{
    private TargetEnemyUI _TargetEnemyUI;

    private PlayerStateUI _PlayerStateUI;

    public TargetEnemyUI targetEnemyUI => _TargetEnemyUI ??
        (_TargetEnemyUI = GetComponentInChildren<TargetEnemyUI>());

    public PlayerStateUI playerStateUI => _PlayerStateUI ??
        (_PlayerStateUI = GetComponentInChildren<PlayerStateUI>());

    /// <summary>
    /// UI 초기화
    /// </summary>
    /// <param name="playerController"></param>
    public void InitializeUI(GameScenePlayerController playerController)
    {
        // 목표 적 표시 UI 초기화
        targetEnemyUI.InitializeUI(playerController);

        // 플레이어 현재 체력 UI 초기화
        playerStateUI.InitializeUI(playerController);
    }

}
