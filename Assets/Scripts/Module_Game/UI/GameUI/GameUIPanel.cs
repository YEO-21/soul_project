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
    /// UI �ʱ�ȭ
    /// </summary>
    /// <param name="playerController"></param>
    public void InitializeUI(GameScenePlayerController playerController)
    {
        // ��ǥ �� ǥ�� UI �ʱ�ȭ
        targetEnemyUI.InitializeUI(playerController);

        // �÷��̾� ���� ü�� UI �ʱ�ȭ
        playerStateUI.InitializeUI(playerController);
    }

}
