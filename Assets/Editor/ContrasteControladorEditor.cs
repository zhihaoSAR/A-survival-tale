#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ContrasteControlador), true)]
public class ContrasteControladorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ContrasteControlador myScript = (ContrasteControlador)target;

        if (GUILayout.Button("Generar ScriptableObjeto"))
        {

            ColorDefecto color = ScriptableObject.CreateInstance<ColorDefecto>();
            color.fondos = new Color[myScript.fondos.Length];
            color.jugador = new Color[myScript.jugador.Length];
            color.interactuables = new Color[myScript.interactuables.Length];
            for (int i = 0;i< myScript.fondos.Length;i++)
            {
                color.fondos[i] = myScript.fondos[i].color;
            }
            for (int i = 0; i < myScript.jugador.Length; i++)
            {
                color.jugador[i] = myScript.jugador[i].color;
            }
            for (int i = 0; i < myScript.interactuables.Length; i++)
            {
                color.interactuables[i] = myScript.interactuables[i].color;
            }
            string path = "Assets/materiales/colorDefecto.asset";
            ColorDefecto asset = AssetDatabase.LoadMainAssetAtPath(path) as ColorDefecto;
            if (asset != null)
            {
                EditorUtility.CopySerialized(color, asset);

            }
            else
            {
                AssetDatabase.CreateAsset(color, path);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
        if (GUILayout.Button("volver color al defecto"))
        {
            myScript.volverAlColorDefecto();
        }


    }
}
#endif