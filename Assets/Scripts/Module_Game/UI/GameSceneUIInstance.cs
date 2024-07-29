using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSceneUIInstance : UIInstanceBase
{
    //public NpcInteractUIPanel m_NpcInteractUIPanelPrefab;
    private NpcInteractUIPanel _NpcInteractUIPanelPrefab;


    /// <summary>
    /// GameUIPanel ������Ʈ�� ��Ÿ���ϴ�.
    /// </summary>
    public GameUIPanel gameUI { get; private set; }

    public override void InitializeUI(PlayerControllerBase playerController)
    {
        GameScenePlayerController gameScenePlayerController =
            playerController as GameScenePlayerController;

        // Npc ��ȣ�ۿ� �� ǥ�õ� UI ������ �ε�
        _NpcInteractUIPanelPrefab = Resources.Load<NpcInteractUIPanel>(
            "Prefabs/UI/Panel_NpcInteractUI");

        // GameUIPanel �� ã���ϴ�.
        gameUI = GetComponentInChildren<GameUIPanel>();

        // GameUIPanel �ʱ�ȭ
        gameUI.InitializeUI(gameScenePlayerController);
    }

    public NpcInteractUIPanel OpenNpcInteractUI(NpcInfo npcInfo)
    {
        NpcInteractUIPanel interactUIPanel = Instantiate(
            _NpcInteractUIPanelPrefab, transform);

        interactUIPanel.InitializeUI(npcInfo);

        return interactUIPanel;
    }
}
