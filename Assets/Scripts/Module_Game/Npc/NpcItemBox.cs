using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public sealed class NpcItemBox : NpcBase
{

    private NpcInteractUIPanel _UseInteractUI;

    public override void OnInteractStarted(NpcInteractUIPanel useInteractUI)
    {
        base.OnInteractStarted(useInteractUI);

        _UseInteractUI = useInteractUI;

        useInteractUI.ShowSelections(1);
        useInteractUI.SetSelectionLabel(0, "���ڸ� ���� �������� ��´�.");
        useInteractUI.selectButtons[0].onClick.AddListener(CALLBACK_OnSelectButtonClicked);


    }

  

    private void CALLBACK_OnSelectButtonClicked()
    {
       // PlayerController
        GameScenePlayerController playerController =
            SceneManagerBase.instance.sceneInstance.playerController as GameScenePlayerController;

        // PlayerState
        GameScenePlayerState playerState = playerController.playerState as GameScenePlayerState;

        // ���� �������� 5�� �����մϴ�.
        InventoryItemInfo itemInfo = playerState.GetItemInfo("000001");
        itemInfo.itemCount += 5;


        playerState.SetItemInfo(itemInfo);


        // ���� NPC ������Ʈ�� �����մϴ�.
        Destroy(gameObject);


        // ��ȣ�ۿ� ����
        _UseInteractUI.CloseUI();

    }
}
