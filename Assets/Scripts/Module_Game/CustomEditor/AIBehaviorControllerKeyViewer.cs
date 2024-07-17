using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// AI Behavior Controller 를 사용하는 객체의 등록된 키를 노출시키기 위한 
/// 창 클래스입니다.
/// </summary>
public class AIBehaviorControllerKeyViewer : EditorWindow
{
    /// <summary>
    /// 선택된 BehaviorController 컴포넌트 객체를 나타냅니다.
    /// </summary>
    private List<BehaviorController> _SelectedBehaviorControllers = new();

    /// <summary>
    /// 현재 스크롤된 위치를 기록하기 위한 변수
    /// </summary>
    private Vector2 _CurrentScrollPosition;

    [MenuItem("Window/AI/BehaviorController KeyViewer")]
    public static void Initialize()
    {
        // AIBehaviorControllerKeyViewer 창을 띄웁니다.
        AIBehaviorControllerKeyViewer viewerWindowInstance =
            GetWindow<AIBehaviorControllerKeyViewer>();

        // 띄운 창의 최소 크기를 설정합니다.
        viewerWindowInstance.minSize = new Vector2(200.0f, 200.0f);
    }

    private void Update()
    {
        // 선택된 모든 오브젝트 트랜스폼을 얻습니다.
        Transform[] selectedTransformsInWorld = Selection.GetTransforms(
            SelectionMode.TopLevel | 
            SelectionMode.ExcludePrefab | 
            SelectionMode.Editable);

        // 리스트 비우기
        _SelectedBehaviorControllers.Clear();

        // 선택된 오브젝트의 BehaviorController 컴포넌트를 얻습니다.
        foreach (Transform transforms in selectedTransformsInWorld)
        {
            BehaviorController behaviorController = transforms.GetComponent<BehaviorController>();
            if (behaviorController) _SelectedBehaviorControllers.Add(behaviorController);
        }

        // 창을 갱신합니다.
        Repaint();
    }

    private void OnGUI()
    {
        try
        {
            // 스크롤 시작 위치 설정
            _CurrentScrollPosition = GUILayout.BeginScrollView(
                _CurrentScrollPosition, false, true);

            foreach (BehaviorController bhController in _SelectedBehaviorControllers)
            {
                // 선택된 오브젝트 이름을 얻습니다.
                string selectedObjectName = bhController.gameObject.name;

                // 게임 오브젝트 이름 표시용 스타일
                GUIStyle labelStyle_GameObjectName = new();
                labelStyle_GameObjectName.fontSize = 13;
                labelStyle_GameObjectName.fontStyle = FontStyle.Bold;
                labelStyle_GameObjectName.normal.textColor = Color.cyan;

                // 이름을 출력합니다.
                GUILayout.Label(selectedObjectName, labelStyle_GameObjectName);

                // 키와 값을 이곳에서 출력합니다.
                foreach (KeyValuePair<string, object> keyInfo in bhController.keys)
                {
                    string keyName = keyInfo.Key;
                    object keyValue = keyInfo.Value;
                    bool valueIsEmpty = keyValue == null;

                    GUILayout.Label($"{(valueIsEmpty ? "○" : "●")}[{keyName}] {keyValue}");
                }

                GUILayout.Space(20.0f);
            }
        }
        catch (System.Exception) 
        {
            _SelectedBehaviorControllers.Clear();
        }

        // 스크롤 끝 위치 설정
        GUILayout.EndScrollView();
    }
}
#endif