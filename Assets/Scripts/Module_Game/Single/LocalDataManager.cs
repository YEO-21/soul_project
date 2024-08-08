using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LocalDataManager : ManagerClassBase<LocalDataManager>
{
   public SettingData settingData { get; set; } = new SettingData();

   public void WriteSettingData()
    {
        // 현재 설정 내용을 JSON 문자열로 변환합니다.

        // 저장합니다.

        // 적용시킵니다.
        Screen.SetResolution(
            settingData.resolution.width,
            settingData.resolution.height,
            settingData.windowMode,
            settingData.resolution.refreshRateRatio);
    }


}
