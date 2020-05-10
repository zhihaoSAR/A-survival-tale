using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Controlador : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys;
    public MyInputModule inputModule;
    public DatosSistema datosSistema;
    public UIControl uicontrol;
    void Start()
    {
        SistemaGuardar.cargarDatosSistema(out datosSistema);
        keys = datosSistema.keys;
        Mappear();
        
        
    }
    public void Mappear()
    {
        inputModule.arriba = keys["arriba"];
        inputModule.abajo = keys["abajo"];
        inputModule.izquirda = keys["izquierda"];
        inputModule.derecha = keys["derecha"];
        inputModule.confirmar = keys["confirmar"];
        inputModule.cancelar = keys["cancelar"];
        inputModule.A = keys["A"];
        inputModule.B = keys["B"];
    }
    public void pausaControl(bool pausa)
    {
        inputModule.canControl = !pausa;
    }

}
