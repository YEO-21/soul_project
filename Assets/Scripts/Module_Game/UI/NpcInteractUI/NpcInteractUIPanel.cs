using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcInteractUIPanel : MonoBehaviour
{
    [Header("# �̸� �ؽ�Ʈ")]
    public TMP_Text m_NameText;

    [Header("# ��ȭ ���� �ؽ�Ʈ")]
    public TMP_Text m_DialogText;



    public RectTransform rectTransform => transform as RectTransform;


    /// <summary>
    /// ��ȭ���� Npc ������ ��Ÿ���ϴ�.
    /// </summary>
    private NpcInfo _NpcInfo;


   public void InitializeUI(NpcInfo npcInfo)
    {
        // Npc ���� ����
        _NpcInfo = npcInfo;

        // RectTransform �ʱ�ȭ
        InitializeRectTransformInfo();

        // ��ȭ ���� �ʱ�ȭ
        InitializeDialogInfo();

        Debug.Log($"Npc �̸� : {_NpcInfo.m_Name}");
        Debug.Log($"��ȭ ���� : {_NpcInfo.m_DefaultDialog}");
    }

    private void InitializeRectTransformInfo()
    {
        // Anchor ����
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

        // Pivot ����
        rectTransform.pivot = Vector2.one * 0.5f;

        //Scale ����
        rectTransform.localScale = Vector3.one;

        // ��ü ä��� ��Ŀ����� Left/Top �� ���� anchorPosition X/Y �� �ش�
        rectTransform.anchoredPosition = Vector2.zero;

        // ��ü ä��� ��Ŀ����� Right/Bottom �� ���� Width/Height �� �ش�
        rectTransform.sizeDelta = Vector2.zero;
    }

    private void InitializeDialogInfo()
    {
        m_NameText.text = _NpcInfo.m_Name;
        m_DialogText.text = _NpcInfo.m_DefaultDialog;
    }
}
