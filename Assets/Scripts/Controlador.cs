﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controlador : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys;
    public MyInputModule inputModule;
    public DatosSistema datosSistema;
    public UIControl uicontrol;
    static float tiempoTransicion = 0.15f;
    public EventSystem eventSystem;

    public Menu canvasActual;
    TMP_FontAsset tcm,openDyslexic;
    [SerializeField]
    public AudioMixer audioMixer;
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
    public bool canControl = true;
    public Image I_registrando;
    Action<Color> acabado;
    //--------------salir-------------
    bool cerrandoJuego = false;
    public RectTransform panelSalir;
    public TextMeshProUGUI[] salirTextos;
    public GameObject[] salirNav;
    bool cerrar = false;

    void Start()
    {
        SistemaGuardar.cargarDatosSistema(out datosSistema);
        keys = datosSistema.keys;
        Mappear();
        Application.wantsToQuit += cerrarJuego;
        canvasActual.abrirMenu(this, 0);
    }
    void Update()
    {
        
        if(elegiendoColor && canControl)
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
                registraControl();
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
            if(Input.GetKeyDown(keys["confirmar"]) || Input.GetKeyDown(keys["A"]))
            {
                cerrarElegirColor(true);
            }
            else if (Input.GetKeyDown(keys["cancelar"]) || Input.GetKeyDown(keys["B"]) || Input.GetMouseButtonUp(1))
            {
                cerrarElegirColor(false);
            }
        }
    }

    public void cerrarElegirColor(bool confirmar)
    {
        registraControl();
        if(confirmar)
        {
            acabado(preview.color);
        }
        panelElegirColor.gameObject.SetActive(false);
        panelElegirColor.parent = transform;
        canNavegar = true;
        uiCanControl = true;
        elegiendoColor = false;
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
        if (modo == 0)
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
        rect.anchoredPosition = Vector2.zero;
        yield return null;
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
    public void ElegirColor(Vector2 pos,Action<Color> acabado)
    {
        canNavegar = false;
        uiCanControl = false;
        panelElegirColor.gameObject.SetActive(true);
        panelElegirColor.parent = canvasActual.transform;
        panelElegirColor.localScale = Vector3.zero;
        rect_color.anchoredPosition = pos;
        Debug.Log(pos);
        StartCoroutine("PopUp",panelElegirColor);
        elegiendoColor = true ;
        mainCamera = Camera.main;
        MoverMarca(Vector2.zero);
        tamanyoColor = new Vector2(tex_color.width,tex_color.height);
        this.acabado = acabado;
    }
    void MoverMarca(Vector2 pos)
    {
        
        pos.x = Mathf.Clamp(pos.x, 0, tamanyoColor.x);
        pos.y = Mathf.Clamp(pos.y, 0, tamanyoColor.y);
        marca.rectTransform.localPosition = pos;
        preview.color = tex_color.GetPixel((int)pos.x,(int)pos.y);
        float grayscale = 1- preview.color.grayscale;
        marca.color = new Color(grayscale, grayscale, grayscale);
    }
    public TMP_FontAsset getFont(int tipo)
    {
        if(tipo == 0)
        {
            if(tcm != null)
            {
                return tcm;
            }
            else
            {
                tcm = Resources.Load<TMP_FontAsset>("TCM");
                return tcm;
            }
        }
        else
        {
            if(openDyslexic != null)
            {
                return openDyslexic;
            }
            else
            {
                openDyslexic = Resources.Load<TMP_FontAsset>("OpenDyslexic");
                return openDyslexic;
            }
        }
        
    }

    public void CambiarModoSonido(int modo)
    {
        if(modo == 0)
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Stereo;
        }
        else
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Mono;
        }

    }
    //0:ambiente 1: peligro 2:interaccion 3:interfaz 4:paso
    public void ModificarVolumen(int tipo,float vol)
    {
        float dB = (vol / 100 * 80) - 80;
        Debug.Log("soy controlador volumen es" + vol);
        switch(tipo)
        {
            case 0:
                audioMixer.SetFloat("AmbienteVol", dB);
                datosSistema.ambienteVol = vol;
                break;
            case 1:
                audioMixer.SetFloat("PeligroVol", dB);
                datosSistema.peligroVol = vol;
                break;
            case 2:
                audioMixer.SetFloat("InteraccionVol", dB);
                datosSistema.interaccionVol = vol;
                break;
            case 3:
                audioMixer.SetFloat("InterfazVol", dB);
                datosSistema.interfazVol = vol;
                break;
            case 4:
                audioMixer.SetFloat("PasoVol", dB);
                datosSistema.pasoVol = vol;
                break;
        }
    }

    public void CambiarControl(int tipo)
    {
        datosSistema.tipoControl = tipo;
        if(tipo == 2)
        {
            uicontrol.dosBotonModo = true;
            GameObject[] nav;
            if(canvasActual.getActualNavegable(out nav))
            {
                uicontrol.initNavegacion(nav);
            }
        }
        else
        {
            uicontrol.dosBotonModo = false;
            uicontrol.LimpiarNav();
        }
    }

    public void registraControl()
    {
        if (datosSistema.inputTime == 0 || !canControl)
            return;

        StartCoroutine("registrandoControl");
        
    }
    IEnumerator registrandoControl()
    {
        canControl = false;
        float now = 0;
        I_registrando.gameObject.SetActive(true);
        I_registrando.transform.parent = canvasActual.transform;
        while(now <= datosSistema.inputTime)
        {
            now += Time.unscaledDeltaTime;
            I_registrando. fillAmount = now / datosSistema.inputTime;
            yield return null;
        }
        I_registrando.rectTransform.parent = this.transform;
        I_registrando.gameObject.SetActive(false);
        canControl = true;
    }

    public bool cerrarJuego()
    {
        if (cerrar)
            return true;

        if (cerrandoJuego)
            return false;
        
        cerrandoJuego = true;
        TMP_FontAsset fuente = getFont(datosSistema.tipoFuente);

        panelSalir.gameObject.SetActive(true);
        foreach(TextMeshProUGUI text in salirTextos)
        {
            text.font = fuente;
        }
        uicontrol.apilarNavegacion(salirNav);
        panelSalir.parent = canvasActual.transform;
        StartCoroutine(PopUp(panelSalir));
        return false;
    }
    public void cerrarJuego(bool cerrar)
    {
        if(cerrar)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
                this.cerrar = true;
                Application.Quit();
            #endif
        }
        else
        {
            panelSalir.parent = transform;
            panelSalir.gameObject.SetActive(false);
            cerrandoJuego = false;
        }
    }

    public void cancelar()
    {
        if(uiCanControl)
            canvasActual.cancelar();
    }

    public void iniNavegacion(GameObject[] nav)
    {
        uicontrol.initNavegacion(nav);
    }
}