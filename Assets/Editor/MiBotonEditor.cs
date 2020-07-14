using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

using UnityEngine;
[CustomEditor(typeof(MiUiBoton))]
public class MiBotonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {

        MiUiBoton targetMiUiBoton = (MiUiBoton)target;
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("flecha");
        targetMiUiBoton.flecha = EditorGUILayout.ObjectField(targetMiUiBoton.flecha, typeof(RectTransform)) as RectTransform;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("posicion");
        targetMiUiBoton.posicion = EditorGUILayout.ObjectField(targetMiUiBoton.posicion, typeof(RectTransform)) as RectTransform;
        EditorGUILayout.EndHorizontal();
        EditorUtility.SetDirty(targetMiUiBoton);
        base.OnInspectorGUI();
    }
}
#endif
