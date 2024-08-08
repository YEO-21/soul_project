using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameSettingMenuPanel : MonoBehaviour
{
    [Header("# �ػ� Dropdown")]
    public TMP_Dropdown m_ResolutionDropdown;

    [Header("# â��� Dropdown")]
    public TMP_Dropdown m_WindowModeDropdown;

    [Header("# Ȯ�� ��ư")]
    public Button m_OkButton;

    [Header("# ��� ��ư")]
    public Button m_CancelButton;



    private SettingData _SettingData;

    private List<Resolution> _Resolutions = new List<Resolution>();

    public void InitializeUI()
    {
        // ���� ������ �������� ����ϴ�.
        _SettingData = LocalDataManager.instance.settingData;


        // �ػ� Dropdown �ʱ�ȭ
        InitializeResolutionDropdown();

        // â��� Dropdown �ʱ�ȭ
        InitializeWindowModeDropdown();

        m_ResolutionDropdown.onValueChanged.AddListener(CALLBACK_OnResolutionDropdownSelected);
        m_WindowModeDropdown.onValueChanged.AddListener(CALLBACK_OnWindowModeDropdownSelected);

        m_OkButton.onClick.AddListener(CALLBACK_OnOkButtonClicked);
        m_CancelButton.onClick.AddListener(CALLBACK_OnCancelButtonClicked);
    }

    /// <summary>
    /// �ػ� Dropdown �ʱ�ȭ
    /// </summary>
    private void InitializeResolutionDropdown()
    {
        m_ResolutionDropdown.options.Clear();

        // ����� �� �ִ� �ػ󵵸� ��� Ȯ���մϴ�.
       foreach (Resolution resolution in Screen.resolutions)
       {
            string resolutionOption = 
                $"[{resolution.width} X {resolution.height}] {resolution.refreshRateRatio}Hz";


            _Resolutions.Add(resolution);
            m_ResolutionDropdown.options.Add(new(resolutionOption));
            //m_ResolutionDropdown.AddOptions();

            // ���� �ɼ��� ã�� ��� ǥ���մϴ�.
            if(resolution.width == _SettingData.resolution.width &&
                resolution.height == _SettingData.resolution.height)
            {
                m_ResolutionDropdown.value = _Resolutions.Count - 1;
            }
       }



    }

    /// <summary>
    /// â��� Dropdown �ʱ�ȭ
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
    /// �ػ� �ɼ� ���� �ݹ�
    /// </summary>
    /// <param name="itemIndex"></param>
    private void CALLBACK_OnResolutionDropdownSelected(int itemIndex)
    {
        // ���� ���õ� �ػ� �ɼ��� ����ϴ�.
        Resolution selectedResolution = _Resolutions[itemIndex];

        // �ػ� ������ �����մϴ�.
        _SettingData.resolution = selectedResolution;
    }

    private void CALLBACK_OnWindowModeDropdownSelected(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0: // Ǯ��ũ�� ���
                _SettingData.windowMode = FullScreenMode.FullScreenWindow;
                break;

            case 1: // �׵θ� ���� â ȭ��
                _SettingData.windowMode = FullScreenMode.MaximizedWindow;
                break;

            case 2: // â ���
                _SettingData.windowMode = FullScreenMode.Windowed;
                break;
        }

    }

    private void CALLBACK_OnOkButtonClicked()
    {
        LocalDataManager.instance.settingData = _SettingData;

        // ����
        LocalDataManager.instance.WriteSettingData();
    }

    private void CALLBACK_OnCancelButtonClicked()
    {

    }
}
