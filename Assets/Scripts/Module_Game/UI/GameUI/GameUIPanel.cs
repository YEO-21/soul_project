using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameUIPanel : MonoBehaviour
{
    private TargetEnemyUI _TargetEnemyUI;

    private TargetEnemyUI targetEnemyUI => _TargetEnemyUI ??
        (_TargetEnemyUI = GetComponentInChildren<TargetEnemyUI>());

    /// <summary>
    /// UI �ʱ�ȭ
    /// </summary>
    /// <param name="playerController"></param>
    public void InitializeUI(GameScenePlayerController playerController)
    {
        // ��ǥ �� ǥ�� UI �ʱ�ȭ
        targetEnemyUI.InitializeUI(playerController);
    }

}
