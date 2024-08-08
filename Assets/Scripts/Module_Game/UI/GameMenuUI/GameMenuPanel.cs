using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameMenuPanel : MonoBehaviour
{
    [Header("# 설정 패널")]
    public GameSettingMenuPanel m_SettingMenuPanel;

    public void InitializeUI()
    {
        m_SettingMenuPanel.InitializeUI();
    }
}
