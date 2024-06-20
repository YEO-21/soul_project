using System.Collections;
using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
using UnityEngine;
[CustomEditor(typeof(BehaviorController), true)]
public sealed class BehaviorControllerViewerOpenButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

       
        if (GUILayout.Button("키 뷰어 창 열기", GUILayout.Height(50.0f)))
        {
            AIBehaviorControllerKeyViewer.Initialize();  
        }


    }



}
#endif