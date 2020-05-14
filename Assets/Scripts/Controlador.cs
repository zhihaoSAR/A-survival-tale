using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controlador : MonoBehaviour
{


    public static Dictionary<string, KeyCode> keys;
    public MyInputModule inputModule;
    public DatosSistema datosSistema;
    public UIControl uicontrol;
    static float tiempoTransicion = 0.15f;

    public Canvas canvasActual;
    //----------elegir color--------------
    public RectTransform rect_color;
    public Texture2D tex_color;
    public Image marca;
    public RectTransform panelElegirColor;
    public Image preview;
    bool elegiendoColor =false;
    Action<Color> completado;
    Camera mainCamera;
    Vector2 tamanyoColor;
    bool persigueRaton = false;
    int velocidad = 100;


    void Start()
    {
        SistemaGuardar.cargarDatosSistema(out datosSistema);
        keys = datosSistema.keys;
        Mappear();
        
    }
    void Update()
    {
        
        if(elegiendoColor)
        {

            Vector2 pos;
            if (Input.GetMouseButtonDown(0))
            {
                pos = Input.mousePosition;
                pos = rect_color.transform.InverseTransformPoint(pos);
                if (pos.x >= 0 && pos.x <= tamanyoColor.x  && 
                    pos.y >= 0 && pos.y <= tamanyoColor.y )
                        persigueRaton = true;
            }
            if(Input.GetMouseButtonUp(0))
            {

                persigueRaton = false;
            }
            if (persigueRaton)
            {
                pos = Input.mousePosition;
                pos = rect_color.transform.InverseTransformPoint(pos);
                MoverMarca(pos);
                return;
            }
            pos = marca.rectTransform.localPosition;
                
            if (Input.GetKey(keys["arriba"]))
            {
                pos.y += velocidad*Time.unscaledDeltaTime;
            }
            if (Input.GetKey(keys["abajo"]))
            {
                pos.y -= velocidad * Time.unscaledDeltaTime;
            }
            if (Input.GetKey(keys["izquierda"]))
            {
                pos.x -= velocidad * Time.unscaledDeltaTime;
            }
            if (Input.GetKey(keys["derecha"]))
            {
                pos.x += velocidad * Time.unscaledDeltaTime;
            }
            
            
            if (uicontrol.dosBotonModo)
            {
                pos.x += velocidad * Time.unscaledDeltaTime;
                if (pos.x > tamanyoColor.x)
                {
                    pos.x = 0;
                    pos.y += 66;
                    if (pos.y > 400)
                        pos.y = 0;
                }
            }
            MoverMarca(pos);
        }
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

    public bool uiCanControl
    {
        get { return inputModule.canControl; }
        set { inputModule.canControl = value; }
    }
    public bool canNavegar
    {
        get { return !uicontrol.pausaNav; }
        set { uicontrol.pausaNav = !value; }
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

    IEnumerator PopUp(RectTransform rect)
    {
        float now = 0;
        bool antes_uiCanControl = uiCanControl;
        bool antes_canNavegar = canNavegar;
        uiCanControl = false;
        canNavegar = false;
        while(now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            rect.localScale = Vector3.one * (now / tiempoTransicion);
            yield return null;
        }
        rect.localScale = Vector3.one;
        uiCanControl = antes_uiCanControl;
        canNavegar = antes_canNavegar;
    }

    //public void ElegirColor(Vector2 pos,Action<Color> acabado)
    public void ElegirColor()
    {
        canNavegar = false;
        uiCanControl = false;
        panelElegirColor.gameObject.SetActive(true);
        panelElegirColor.parent = canvasActual.transform;
        panelElegirColor.localScale = Vector3.zero;
        rect_color.position = new Vector2(200,400);
        StartCoroutine("PopUp",panelElegirColor);
        elegiendoColor = true ;
        mainCamera = Camera.main;
        tamanyoColor = new Vector2(tex_color.width,tex_color.height);
        Debug.Log(tamanyoColor);
    }
    void MoverMarca(Vector2 pos)
    {
        
        pos.x = Mathf.Clamp(pos.x, 0, tamanyoColor.x);
        pos.y = Mathf.Clamp(pos.y, 0, tamanyoColor.y);
        marca.rectTransform.localPosition = pos;
        preview.color = tex_color.GetPixel((int)pos.x,(int)pos.y);
    }

}
