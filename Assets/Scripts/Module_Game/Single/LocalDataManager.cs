using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LocalDataManager : ManagerClassBase<LocalDataManager>
{
   public SettingData settingData { get; set; } = new SettingData();

   public void WriteSettingData()
    {
        // ���� ���� ������ JSON ���ڿ��� ��ȯ�մϴ�.

        // �����մϴ�.

        // �����ŵ�ϴ�.
        Screen.SetResolution(
            settingData.resolution.width,
            settingData.resolution.height,
            settingData.windowMode,
            settingData.resolution.refreshRateRatio);
    }


}
