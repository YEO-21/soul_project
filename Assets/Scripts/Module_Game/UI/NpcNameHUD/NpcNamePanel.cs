using GameModule;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class NpcNamePanel : MonoBehaviour
{
    [Header("# �̸� �ؽ�Ʈ")]
    public TMP_Text m_NameText;

    /// <summary>
    /// ��ġ�� �����ϱ� ���� �� Ÿ��
    /// </summary>
    private Transform _ViewTargetTransform;

    /// <summary>
    /// Y�� ������
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
        // ���� ����
        UpdateOpacity();

        // ��ġ ����
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
    /// HUD ��ġ�� �����մϴ�.
    /// </summary>
    private void UpdateHUDPosition()
    {
        // ȭ�鿡 ǥ�õǴ� NPC ��ġ�� ����ϴ�.
        Vector3 screenPosition = _CameraComponent.WorldToViewportPoint(
            _ViewTargetTransform.position + (Vector3.up * _YOffset));

        Vector3 newPosition = new Vector3(
            screenPosition.x * Constants.SCREEN_SIZE.x,
            screenPosition.y * Constants.SCREEN_SIZE.y,
            screenPosition.z);

        rectTransform.anchoredPosition = newPosition;
    }

}
