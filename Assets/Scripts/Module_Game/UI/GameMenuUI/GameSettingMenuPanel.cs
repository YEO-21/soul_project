using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameSettingMenuPanel : MonoBehaviour
{
    [Header("# 해상도 Dropdown")]
    public TMP_Dropdown m_ResolutionDropdown;

    [Header("# 창모드 Dropdown")]
    public TMP_Dropdown m_WindowModeDropdown;

    [Header("# 확인 버튼")]
    public Button m_OkButton;

    [Header("# 취소 버튼")]
    public Button m_CancelButton;



    private SettingData _SettingData;

    private List<Resolution> _Resolutions = new List<Resolution>();

    public void InitializeUI()
    {
        // 현재 설정된 설정값을 얻습니다.
        _SettingData = LocalDataManager.instance.settingData;


        // 해상도 Dropdown 초기화
        InitializeResolutionDropdown();

        // 창모드 Dropdown 초기화
        InitializeWindowModeDropdown();

        m_ResolutionDropdown.onValueChanged.AddListener(CALLBACK_OnResolutionDropdownSelected);
        m_WindowModeDropdown.onValueChanged.AddListener(CALLBACK_OnWindowModeDropdownSelected);

        m_OkButton.onClick.AddListener(CALLBACK_OnOkButtonClicked);
        m_CancelButton.onClick.AddListener(CALLBACK_OnCancelButtonClicked);
    }

    /// <summary>
    /// 해상도 Dropdown 초기화
    /// </summary>
    private void InitializeResolutionDropdown()
    {
        m_ResolutionDropdown.options.Clear();

        // 사용할 수 있는 해상도를 모두 확인합니다.
       foreach (Resolution resolution in Screen.resolutions)
       {
            string resolutionOption = 
                $"[{resolution.width} X {resolution.height}] {resolution.refreshRateRatio}Hz";


            _Resolutions.Add(resolution);
            m_ResolutionDropdown.options.Add(new(resolutionOption));
            //m_ResolutionDropdown.AddOptions();

            // 현재 옵션을 찾은 경우 표시합니다.
            if(resolution.width == _SettingData.resolution.width &&
                resolution.height == _SettingData.resolution.height)
            {
                m_ResolutionDropdown.value = _Resolutions.Count - 1;
            }
       }



    }

    /// <summary>
    /// 창모드 Dropdown 초기화
    /// </summary>
    private void InitializeWindowModeDropdown()
    {
        switch (_SettingData.windowMode)
        {
            case FullScreenMode.FullScreenWindow:
                m_WindowModeDropdown.value = 0;
                break;
            case FullScreenMode.MaximizedWindow:
                m_WindowModeDropdown.value = 1;
                break;
            case FullScreenMode.Windowed:
                m_WindowModeDropdown.value = 2;
                break;
        }
    }

    /// <summary>
    /// 해상도 옵션 선택 콜백
    /// </summary>
    /// <param name="itemIndex"></param>
    private void CALLBACK_OnResolutionDropdownSelected(int itemIndex)
    {
        // 현재 선택된 해상도 옵션을 얻습니다.
        Resolution selectedResolution = _Resolutions[itemIndex];

        // 해상도 설정을 적용합니다.
        _SettingData.resolution = selectedResolution;
    }

    private void CALLBACK_OnWindowModeDropdownSelected(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0: // 풀스크린 모드
                _SettingData.windowMode = FullScreenMode.FullScreenWindow;
                break;

            case 1: // 테두리 없는 창 화면
                _SettingData.windowMode = FullScreenMode.MaximizedWindow;
                break;

            case 2: // 창 모드
                _SettingData.windowMode = FullScreenMode.Windowed;
                break;
        }

    }

    private void CALLBACK_OnOkButtonClicked()
    {
        LocalDataManager.instance.settingData = _SettingData;

        // 적용
        LocalDataManager.instance.WriteSettingData();
    }

    private void CALLBACK_OnCancelButtonClicked()
    {

    }
}
