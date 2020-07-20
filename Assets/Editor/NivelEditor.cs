#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Nivel),true)]
public class NivelEditor : Editor
{
    string nivel;
    public override void OnInspectorGUI()
    {
        nivel = EditorGUILayout.TextField("nivel", nivel);
        DrawDefaultInspector();

        Nivel myScript = (Nivel)target;
        
        if (GUILayout.Button("Generar ScriptableObjeto"))
        {
            NivelConfig datos = myScript.generarConfig(nivel);
            NivelConfigObj config = ScriptableObject.CreateInstance<NivelConfigObj>();
            NivelConfigObj.config2Obj(ref config, datos);
            string path = "Assets/NivelConfig/"+nivel+".asset";
            NivelConfigObj asset = AssetDatabase.LoadMainAssetAtPath(path) as NivelConfigObj;
            if (asset != null)
            {
                EditorUtility.CopySerialized(config, asset);
                
            }
            else
            {
                AssetDatabase.CreateAsset(config, path);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
    }
}
#endif