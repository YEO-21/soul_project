using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSceneUIInstance : UIInstanceBase
{
    //public NpcInteractUIPanel m_NpcInteractUIPanelPrefab;
    private NpcInteractUIPanel _NpcInteractUIPanelPrefab;
    private GameMenuPanel _GameMenuPanelPrefab;



    /// <summary>
    /// GameUIPanel ������Ʈ�� ��Ÿ���ϴ�.
    /// </summary>
    public GameUIPanel gameUI { get; private set; }

    /// <summary>
    /// HUD �г��� ��Ÿ���ϴ�.
    /// </summary>
    public GameHUDPanel hudUI { get; private set; }

    public override void InitializeUI(PlayerControllerBase playerController)
    {
        GameScenePlayerController gameScenePlayerController =
            playerController as GameScenePlayerController;

        // Npc ��ȣ�ۿ� �� ǥ�õ� UI ������ �ε�
        _NpcInteractUIPanelPrefab = Resources.Load<NpcInteractUIPanel>(
            "Prefabs/UI/Panel_NpcInteractUI");

        _GameMenuPanelPrefab = Resources.Load<GameMenuPanel>(
            "Prefabs/UI/Panel_Menu");

        // GameUIPanel �� ã���ϴ�.
        gameUI = GetComponentInChildren<GameUIPanel>();

        // HUD �г��� ã���ϴ�.
        hudUI = GetComponentInChildren<GameHUDPanel>();

        // GameUIPanel �ʱ�ȭ
        gameUI.InitializeUI(gameScenePlayerController);
    }

    public NpcInteractUIPanel OpenNpcInteractUI(NpcInfo npcInfo)
        => OpenNpcInteractUI<NpcInteractUIPanel>(npcInfo);

    public T OpenNpcInteractUI<T>(NpcInfo npcInfo,
        NpcInteractUIPanel interactUIPrefab = null) 
        where T : NpcInteractUIPanel
    {
        T interactUIPanel = Instantiate(
            (interactUIPrefab) ? interactUIPrefab : _NpcInteractUIPanelPrefab, 
            transform).GetComponent<T>();

        interactUIPanel.InitializeUI(npcInfo);

        return interactUIPanel;
    }

    public GameMenuPanel OpenGameMenu()
    {
        GameMenuPanel gameMenuPanel = Instantiate(_GameMenuPanelPrefab, transform);
        gameMenuPanel.InitializeUI();

        return gameMenuPanel;
    }
}
