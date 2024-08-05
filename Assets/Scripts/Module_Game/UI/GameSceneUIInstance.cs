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
}
