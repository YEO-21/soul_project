using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameHUDPanel : MonoBehaviour
{
    [Header("# Npc ¿Ã∏ß HUD «¡∏Æ∆’")]
    public NpcNamePanel m_NpcNamePanelPrefab;


    public NpcNamePanel CreateNpcNameHUD(string name, Transform viewTarget, float yOffset)
    {
        NpcNamePanel npcNameHUD = Instantiate(m_NpcNamePanelPrefab, transform);
        npcNameHUD.InitializeUI(name, viewTarget, yOffset);

        return npcNameHUD;
    }

}
