using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// AI Behavior Controller �� ����ϴ� ��ü�� ��ϵ� Ű�� �����Ű�� ���� 
/// â Ŭ�����Դϴ�.
/// </summary>
public class AIBehaviorControllerKeyViewer : EditorWindow
{
    /// <summary>
    /// ���õ� BehaviorController ������Ʈ ��ü�� ��Ÿ���ϴ�.
    /// </summary>
    private List<BehaviorController> _SelectedBehaviorControllers = new();

    /// <summary>
    /// ���� ��ũ�ѵ� ��ġ�� ����ϱ� ���� ����
    /// </summary>
    private Vector2 _CurrentScrollPosition;

    [MenuItem("Window/AI/BehaviorController KeyViewer")]
    public static void Initialize()
    {
        // AIBehaviorControllerKeyViewer â�� ���ϴ�.
        AIBehaviorControllerKeyViewer viewerWindowInstance =
            GetWindow<AIBehaviorControllerKeyViewer>();

        // ��� â�� �ּ� ũ�⸦ �����մϴ�.
        viewerWindowInstance.minSize = new Vector2(200.0f, 200.0f);
    }

    private void Update()
    {
        // ���õ� ��� ������Ʈ Ʈ�������� ����ϴ�.
        Transform[] selectedTransformsInWorld = Selection.GetTransforms(
            SelectionMode.TopLevel | 
            SelectionMode.ExcludePrefab | 
            SelectionMode.Editable);

        // ����Ʈ ����
        _SelectedBehaviorControllers.Clear();

        // ���õ� ������Ʈ�� BehaviorController ������Ʈ�� ����ϴ�.
        foreach (Transform transforms in selectedTransformsInWorld)
        {
            BehaviorController behaviorController = transforms.GetComponent<BehaviorController>();
            if (behaviorController) _SelectedBehaviorControllers.Add(behaviorController);
        }

        // â�� �����մϴ�.
        Repaint();
    }

    private void OnGUI()
    {
        try
        {
            // ��ũ�� ���� ��ġ ����
            _CurrentScrollPosition = GUILayout.BeginScrollView(
                _CurrentScrollPosition, false, true);

            foreach (BehaviorController bhController in _SelectedBehaviorControllers)
            {
                // ���õ� ������Ʈ �̸��� ����ϴ�.
                string selectedObjectName = bhController.gameObject.name;

                // ���� ������Ʈ �̸� ǥ�ÿ� ��Ÿ��
                GUIStyle labelStyle_GameObjectName = new();
                labelStyle_GameObjectName.fontSize = 13;
                labelStyle_GameObjectName.fontStyle = FontStyle.Bold;
                labelStyle_GameObjectName.normal.textColor = Color.cyan;

                // �̸��� ����մϴ�.
                GUILayout.Label(selectedObjectName, labelStyle_GameObjectName);

                // Ű�� ���� �̰����� ����մϴ�.
                foreach (KeyValuePair<string, object> keyInfo in bhController.keys)
                {
                    string keyName = keyInfo.Key;
                    object keyValue = keyInfo.Value;
                    bool valueIsEmpty = keyValue == null;

                    GUILayout.Label($"{(valueIsEmpty ? "��" : "��")}[{keyName}] {keyValue}");
                }

                GUILayout.Space(20.0f);
            }
        }
        catch (System.Exception) 
        {
            _SelectedBehaviorControllers.Clear();
        }

        // ��ũ�� �� ��ġ ����
        GUILayout.EndScrollView();
    }
}
#endif