using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcInteractUIPanel : MonoBehaviour
{
    private TMP_Text _NameText;
    private TMP_Text _DialogText;

    public Button quitButton { get; private set; }
    public List<Button> selectButtons { get; private set; }

    #region 이벤트
    // 이 UI 가 닫힐 경우 발생하는 이벤트
    public event System.Action onUIClosed;
    #endregion


    public RectTransform rectTransform => transform as RectTransform;

    /// <summary>
    /// 대화중인 Npc 정보를 나타냅니다.
    /// </summary>
    private NpcInfo _NpcInfo;

    public virtual void InitializeUI(NpcInfo npcInfo)
    {
        // UI 요소 초기화
        InitializeUIElements();

        ShowSelections(0);

        // Npc 정보 설정
         _NpcInfo = npcInfo;

        // RectTransform 초기화
        InitializeRectTransformInfo();

        // 대화 정보 초기화
        InitializeDialogInfo();

        // 닫힘 버튼 이벤트 설정
        quitButton.onClick.AddListener(CALLBACK_OnQuitButtonClicked);
    }

    public virtual void CloseUI()
    {
        onUIClosed?.Invoke();
        Destroy(gameObject);
    }

    public void ShowSelections(int selCount)
    {
        for (int i = 0; i < selCount; ++i)
        {
            selectButtons[i].gameObject.SetActive(true);
        }
    }

    public void SetSelectionLabel(int index, string label)
    {
        TMP_Text labelText = selectButtons[index].GetComponentInChildren<TMP_Text>();
        labelText.text = label;
    }

    private void InitializeUIElements()
    {
        Transform defaultDialogParentUI = transform.Find("Panel_Dialog");
        _NameText = defaultDialogParentUI.Find("Text_InteractableName").GetComponent<TMP_Text>();
        _DialogText = defaultDialogParentUI.Find("Text_Dialog").GetComponent<TMP_Text>();

        Transform selectionParentUI = transform.Find("Panel_Selections");
        quitButton = selectionParentUI.Find("Button_Quit").GetComponent<Button>();
        selectButtons = new List<Button>();
        for (int i = 0; i < 6; ++i)
        {
            Button findedSelectionButton = selectionParentUI.Find(
                $"Button_Selection0{(i + 1)}").GetComponent<Button>();

            // 기본적으로 비활성화 상태가 되도록 합니다.
            findedSelectionButton.gameObject.SetActive(false);

            selectButtons.Add(findedSelectionButton);
        }
    }

    private void InitializeRectTransformInfo()
    {
        // Anchor 설정
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

        // Pivot 설정
        rectTransform.pivot = Vector2.one * 0.5f;

        // Scale 설정
        rectTransform.localScale = Vector3.one;

        // 전체 채우기 방식에서는 Left/Top 의 값이 anchordPosition X/Y 에 해당
        rectTransform.anchoredPosition = Vector3.zero;

        // 전체 채우기 방식에서는 Right/Bottom 의 값이 Width/Height 에 해당
        rectTransform.sizeDelta = Vector3.zero;
    }

    private void InitializeDialogInfo()
    {
        _NameText.text = _NpcInfo.m_Name;
        _DialogText.text = _NpcInfo.m_DefaultDialog;
    }

    private void CALLBACK_OnQuitButtonClicked()
        => CloseUI();

}
