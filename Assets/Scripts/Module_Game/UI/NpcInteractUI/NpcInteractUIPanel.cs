using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcInteractUIPanel : MonoBehaviour
{
    [Header("# 이름 텍스트")]
    public TMP_Text m_NameText;

    [Header("# 대화 내용 텍스트")]
    public TMP_Text m_DialogText;



    public RectTransform rectTransform => transform as RectTransform;


    /// <summary>
    /// 대화중인 Npc 정보를 나타냅니다.
    /// </summary>
    private NpcInfo _NpcInfo;


   public void InitializeUI(NpcInfo npcInfo)
    {
        // Npc 정보 설정
        _NpcInfo = npcInfo;

        // RectTransform 초기화
        InitializeRectTransformInfo();

        // 대화 정보 초기화
        InitializeDialogInfo();

        Debug.Log($"Npc 이름 : {_NpcInfo.m_Name}");
        Debug.Log($"대화 내용 : {_NpcInfo.m_DefaultDialog}");
    }

    private void InitializeRectTransformInfo()
    {
        // Anchor 설정
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

        // Pivot 설정
        rectTransform.pivot = Vector2.one * 0.5f;

        //Scale 설정
        rectTransform.localScale = Vector3.one;

        // 전체 채우기 방식에서는 Left/Top 의 값이 anchorPosition X/Y 에 해당
        rectTransform.anchoredPosition = Vector2.zero;

        // 전체 채우기 방식에서는 Right/Bottom 의 값이 Width/Height 에 해당
        rectTransform.sizeDelta = Vector2.zero;
    }

    private void InitializeDialogInfo()
    {
        m_NameText.text = _NpcInfo.m_Name;
        m_DialogText.text = _NpcInfo.m_DefaultDialog;
    }
}
