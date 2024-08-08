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

    #region �̺�Ʈ
    // �� UI �� ���� ��� �߻��ϴ� �̺�Ʈ
    public event System.Action onUIClosed;
    #endregion


    public RectTransform rectTransform => transform as RectTransform;

    /// <summary>
    /// ��ȭ���� Npc ������ ��Ÿ���ϴ�.
    /// </summary>
    private NpcInfo _NpcInfo;

    public virtual void InitializeUI(NpcInfo npcInfo)
    {
        // UI ��� �ʱ�ȭ
        InitializeUIElements();

        ShowSelections(0);

        // Npc ���� ����
         _NpcInfo = npcInfo;

        // RectTransform �ʱ�ȭ
        InitializeRectTransformInfo();

        // ��ȭ ���� �ʱ�ȭ
        InitializeDialogInfo();

        // ���� ��ư �̺�Ʈ ����
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

            // �⺻������ ��Ȱ��ȭ ���°� �ǵ��� �մϴ�.
            findedSelectionButton.gameObject.SetActive(false);

            selectButtons.Add(findedSelectionButton);
        }
    }

    private void InitializeRectTransformInfo()
    {
        // Anchor ����
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

        // Pivot ����
        rectTransform.pivot = Vector2.one * 0.5f;

        // Scale ����
        rectTransform.localScale = Vector3.one;

        // ��ü ä��� ��Ŀ����� Left/Top �� ���� anchordPosition X/Y �� �ش�
        rectTransform.anchoredPosition = Vector3.zero;

        // ��ü ä��� ��Ŀ����� Right/Bottom �� ���� Width/Height �� �ش�
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
