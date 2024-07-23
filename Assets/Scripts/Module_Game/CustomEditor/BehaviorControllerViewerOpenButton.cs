using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(BehaviorController), true)]
public sealed class BehaviorControllerViewerOpenButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        if (GUILayout.Button("Ű ��� â ����", GUILayout.Height(50.0f)))
        {
            AIBehaviorControllerKeyViewer.Initialize();
        }
    }

}

#endif