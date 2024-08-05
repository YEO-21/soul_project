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
        useInteractUI.SetSelectionLabel(0, "상자를 열어 아이템을 얻는다.");
        useInteractUI.selectButtons[0].onClick.AddListener(CALLBACK_OnSelectButtonClicked);


    }

  

    private void CALLBACK_OnSelectButtonClicked()
    {
       // PlayerController
        GameScenePlayerController playerController =
            SceneManagerBase.instance.sceneInstance.playerController as GameScenePlayerController;

        // PlayerState
        GameScenePlayerState playerState = playerController.playerState as GameScenePlayerState;

        // 포션 아이템을 5개 지급합니다.
        InventoryItemInfo itemInfo = playerState.GetItemInfo("000001");
        itemInfo.itemCount += 5;


        playerState.SetItemInfo(itemInfo);


        // 상자 NPC 오브젝트를 제거합니다.
        Destroy(gameObject);


        // 상호작용 종료
        _UseInteractUI.CloseUI();

    }
}
