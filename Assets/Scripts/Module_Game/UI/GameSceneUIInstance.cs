using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSceneUIInstance : UIInstanceBase
{
    //public NpcInteractUIPanel m_NpcInteractUIPanelPrefab;
    private NpcInteractUIPanel _NpcInteractUIPanelPrefab;


    /// <summary>
    /// GameUIPanel 컴포넌트를 나타냅니다.
    /// </summary>
    public GameUIPanel gameUI { get; private set; }

    public override void InitializeUI(PlayerControllerBase playerController)
    {
        GameScenePlayerController gameScenePlayerController =
            playerController as GameScenePlayerController;

        // Npc 상호작용 시 표시될 UI 프리팹 로드
        _NpcInteractUIPanelPrefab = Resources.Load<NpcInteractUIPanel>(
            "Prefabs/UI/Panel_NpcInteractUI");

        // GameUIPanel 를 찾습니다.
        gameUI = GetComponentInChildren<GameUIPanel>();

        // GameUIPanel 초기화
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
