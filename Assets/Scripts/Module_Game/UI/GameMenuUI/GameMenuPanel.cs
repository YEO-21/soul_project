using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameMenuPanel : MonoBehaviour
{
    [Header("# ���� �г�")]
    public GameSettingMenuPanel m_SettingMenuPanel;

    public void InitializeUI()
    {
        m_SettingMenuPanel.InitializeUI();
    }
}
