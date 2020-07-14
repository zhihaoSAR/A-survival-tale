using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MiUiToggle))]
public class MiToggleEditor : UnityEditor.UI.ToggleEditor
{
    

    public override void OnInspectorGUI()
    { 

        MiUiToggle targetMiToggle = (MiUiToggle)target;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("flecha");
        targetMiToggle.flecha = EditorGUILayout.ObjectField(targetMiToggle.flecha, typeof(RectTransform)) as RectTransform;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("posicion");
        targetMiToggle.posicion = EditorGUILayout.ObjectField(targetMiToggle.posicion, typeof(RectTransform)) as RectTransform;
        EditorGUILayout.EndHorizontal();


        EditorUtility.SetDirty(targetMiToggle);
        base.OnInspectorGUI();
    }

}
#endif