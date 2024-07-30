using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NpcItemBox : NpcBase
{
    public override void OnInteractStarted(NpcInteractUIPanel useInteractUI)
    {
        base.OnInteractStarted(useInteractUI);

        useInteractUI.ShowSelections(1);
        useInteractUI.selectButtons[0].onClick.AddListener(() => Debug.Log("선택지01 선택되었슴돠!"));
    }
}
