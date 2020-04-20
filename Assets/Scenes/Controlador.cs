using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Controlador : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys;
    public MyInputModule inputModule;
    void Start()
    {
        keys = new Dictionary<string, KeyCode>();
        keys.Add("arriba", KeyCode.W);
        keys.Add("abajo", KeyCode.S);
        keys.Add("izquierda", KeyCode.A);
        keys.Add("derecha", KeyCode.D);
        keys.Add("confirmar", KeyCode.Return);
        keys.Add("cancelar", KeyCode.Escape);
        keys.Add("A", KeyCode.Z);
        keys.Add("B", KeyCode.X);
    }
    public void Mappear()
    {
        Debug.Log(keys["arriba"].ToString());
        inputModule.arriba = keys["arriba"];
        inputModule.abajo = keys["abajo"];
        inputModule.izquirda = keys["izquierda"];
        inputModule.derecha = keys["derecha"];
        inputModule.confirmar = keys["confirmar"];
        inputModule.cancelar = keys["cancelar"];
        inputModule.A = keys["A"];
        inputModule.B = keys["B"];
    }

}
