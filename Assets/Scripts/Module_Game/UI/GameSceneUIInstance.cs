using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSceneUIInstance : UIInstanceBase
{
    //public NpcInteractUIPanel m_NpcInteractUIPanelPrefab;
    private NpcInteractUIPanel _NpcInteractUIPanelPrefab;
    private GameMenuPanel _GameMenuPanelPrefab;



    /// <summary>
    /// GameUIPanel 컴포넌트를 나타냅니다.
    /// </summary>
    public GameUIPanel gameUI { get; private set; }

    /// <summary>
    /// HUD 패널을 나타냅니다.
    /// </summary>
    public GameHUDPanel hudUI { get; private set; }

    public override void InitializeUI(PlayerControllerBase playerController)
    {
        GameScenePlayerController gameScenePlayerController =
            playerController as GameScenePlayerController;

        // Npc 상호작용 시 표시될 UI 프리팹 로드
        _NpcInteractUIPanelPrefab = Resources.Load<NpcInteractUIPanel>(
            "Prefabs/UI/Panel_NpcInteractUI");

        _GameMenuPanelPrefab = Resources.Load<GameMenuPanel>(
            "Prefabs/UI/Panel_Menu");

        // GameUIPanel 를 찾습니다.
        gameUI = GetComponentInChildren<GameUIPanel>();

        // HUD 패널을 찾습니다.
        hudUI = GetComponentInChildren<GameHUDPanel>();

        // GameUIPanel 초기화
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
