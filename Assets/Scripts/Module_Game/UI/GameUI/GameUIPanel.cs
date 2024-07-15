using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameUIPanel : MonoBehaviour
{
    private TargetEnemyUI _TargetEnemyUI;

    private TargetEnemyUI targetEnemyUI => _TargetEnemyUI ??
        (_TargetEnemyUI = GetComponentInChildren<TargetEnemyUI>());

    /// <summary>
    /// UI 초기화
    /// </summary>
    /// <param name="playerController"></param>
    public void InitializeUI(GameScenePlayerController playerController)
    {
        // 목표 적 표시 UI 초기화
        targetEnemyUI.InitializeUI(playerController);
    }

}
