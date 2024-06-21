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

       
        if (GUILayout.Button("Ű ��� â ����", GUILayout.Height(50.0f)))
        {
            AIBehaviorControllerKeyViewer.Initialize();  
        }


    }



}
#endif