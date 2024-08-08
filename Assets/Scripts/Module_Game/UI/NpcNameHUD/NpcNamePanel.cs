using GameModule;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class NpcNamePanel : MonoBehaviour
{
    [Header("# 이름 텍스트")]
    public TMP_Text m_NameText;

    /// <summary>
    /// 위치를 결정하기 위한 뷰 타깃
    /// </summary>
    private Transform _ViewTargetTransform;

    /// <summary>
    /// Y축 오프셋
    /// </summary>
    private float _YOffset;

    private Camera _CameraComponent;

    public RectTransform rectTransform => transform as RectTransform;

    private List<Graphic> _ChildGraphics;

    public void InitializeUI(string npcName, Transform viewTarget, float yOffset)
    {
        _ChildGraphics = new List<Graphic>(GetComponentsInChildren<Graphic>());

        rectTransform.anchorMin = rectTransform.anchorMax = Vector2.zero;
        rectTransform.pivot = new Vector2(0.5f, 0.0f);

        _CameraComponent = Camera.main;

        m_NameText.text = npcName;
        _ViewTargetTransform = viewTarget;
        _YOffset = yOffset;
    }

    private void Update()
    {
        // 투명도 갱신
        UpdateOpacity();

        // 위치 갱신
        UpdateHUDPosition();
    }

    private void UpdateOpacity()
    {
        Vector3 cameraPosition = _CameraComponent.transform.position;
        Vector3 viewTargetPosition = _ViewTargetTransform.position;

        bool visible = Vector3.Distance(cameraPosition, viewTargetPosition) < 
            Constants.HUD_INVISIBLE_DISTANCE;

        foreach(Graphic childGraphic in _ChildGraphics)
        {
            Color currentColor = childGraphic.color;

            currentColor.a = visible ? 1.0f : 0.0f;

            childGraphic.color = currentColor;
        }
    }

    /// <summary>
    /// HUD 위치를 갱신합니다.
    /// </summary>
    private void UpdateHUDPosition()
    {
        // 화면에 표시되는 NPC 위치를 얻습니다.
        Vector3 screenPosition = _CameraComponent.WorldToViewportPoint(
            _ViewTargetTransform.position + (Vector3.up * _YOffset));

        Vector3 newPosition = new Vector3(
            screenPosition.x * Constants.SCREEN_SIZE.x,
            screenPosition.y * Constants.SCREEN_SIZE.y,
            screenPosition.z);

        rectTransform.anchoredPosition = newPosition;
    }

}
