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

    public void cambiarCursor(int tipo, int tamaño)
    {
        string prefijo;
        Vector2 hotspot = Vector2.zero; ;

        if (tipo == 0)
        {
            prefijo = "cursor_";
        }
        else
        {
            prefijo = "cursorEsp_";
            switch (tamaño)
            {
                case 0:
                    hotspot = new Vector2(4, 2);
                    break;
                case 1:
                    hotspot = new Vector2(6, 4);
                    break;
                case 2:
                    hotspot = new Vector2(8, 6);
                    break;
            }
        }
        Texture2D cursor = Resources.Load<Texture2D>(prefijo + tamaño);
        UnityEngine.Cursor.SetCursor(cursor, hotspot, CursorMode.ForceSoftware);
    }

    //1:ventana 0:completa
    public void PantallaCompleta(int modo)
    {
        if (modo == 1)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreen = true;
        }
        datosSistema.pantallaCompleta = modo;
    }

}
