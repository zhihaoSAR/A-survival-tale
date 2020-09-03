using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvaJuego : Menu
{
    public RectTransform P_elegirDirCaja;
    public float tiempoTransicion = 0.2f;
    Coroutine elegirDir;
    [HideInInspector]
    public bool recordando = false;
    public RectTransform recordarInteractuar;
    Image I_recordarInteractuar;
    Coroutine Coroutine_recordarInteractuar;
    public RectTransform minimapa;
    public Mapa mapa;

    public void activaMiniMapa(bool activa)
    {
        minimapa.gameObject.SetActive(activa);
    }


    void Start()
    {

        control = Controlador.control;
        GetComponent<Canvas>().worldCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();
        int tipoControl = control.datosSistema.tipoControl;
        string path = "Indicaciones/";
        if (tipoControl == 0)
        {
            path += "Icono Enter";
        }
        else if (tipoControl == 1)
        {
            path += "Icono Raton";
        }
        else if (tipoControl == 2)
        {
            path += "Icono Boton";
        }
        Debug.Log(path);
        Sprite imagenRecordar = Resources.Load<Sprite>(path);
        I_recordarInteractuar = recordarInteractuar.GetComponent<Image>();
        I_recordarInteractuar.sprite = imagenRecordar;
    }
    void Update()
    {
        if(recordando)
        {
            if((Controlador.posicionRaton() -recordarInteractuar.position).sqrMagnitude <= 3600)
            {
                I_recordarInteractuar.color = new Vector4(1,1,1,0.5f);
            }
            else
            {
                I_recordarInteractuar.color = new Vector4(1, 1, 1, 1);
            }
        }
    }
    public override void abrirMenu( int op)
    {
        Controlador.control.jugadorControlable = false;
        activaMiniMapa(false);
        mapa.inicializar();
        mapa.gameObject.SetActive(true);
        control.S_cambioPagina();
        StartCoroutine(abarirMapa());

    }

    public override void cancelar()
    {
        cerrarMenu(0);
        control.S_cambioPagina();
    }

    public override void cerrarMenu(int op)
    {
        StartCoroutine(cerrarMapa());
        if (control.datosSistema.tipoControl != 1)
        {
            Cursor.visible = false;
        }
    }
    IEnumerator cerrarMapa()
    {
        RectTransform rect_mapa = mapa.GetComponent<RectTransform>();
        control.controlable = false;
        control.navegable = false;
        float now = 0;
        float tiempoInv = 1 / tiempoTransicion;
        yield return null;
        while (now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            rect_mapa.localScale = Vector3.one *(1 - (now * tiempoInv));
            yield return null;
        }
        control.controlable = true;
        control.uiControlable = false;
        control.jugadorControlable = true;
        mapa.gameObject.SetActive(false);
    }
    public override bool getActualNavegable(out GameObject[] nav)
    {
        nav = mapa.navegables;
        return true;
    }

    IEnumerator abarirMapa()
    {
        RectTransform rect_mapa = mapa.GetComponent<RectTransform>() ;
        mapa.transform.localScale = Vector3.zero;
        control.controlable = false;
        control.navegable = false;
        float now = 0;
        float tiempoInv = 1 / tiempoTransicion;
        yield return null;
        while (now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            rect_mapa.localScale = Vector3.one * (now *tiempoInv);
            yield return null;
        }
        control.controlable = true;
        control.uiControlable = true;
        control.navegable = true;
        control.iniNavegacion(mapa.navegables);
        control.eventSystem.firstSelectedGameObject = mapa.navegables[1];
    }
    public void abrirConfiguracion()
    {
        control.abrirConfiguracion();
        control.S_cambioPagina();
    }
    public void volverMenuInicio()
    {
        control.menuInicio();
    }
    public void mostrarDireccion(Vector3 posicionPantalla,Transform caja,Quaternion rotacionCamara,Vector3[] posiciones,Action<Vector3> prepararInt)
    {
        P_elegirDirCaja.anchoredPosition = posicionPantalla;
        P_elegirDirCaja.localEulerAngles = Vector3.zero;
        Vector3 dst = caja.forward;
        dst.y = dst.z;
        dst.z = 0;
        P_elegirDirCaja.localRotation *= Quaternion.FromToRotation( Vector3.up,dst); ;
        P_elegirDirCaja.localRotation *= rotacionCamara;
        for (int i = 0; i< P_elegirDirCaja.childCount; i++)
        {
            Button dir = P_elegirDirCaja.GetChild(i).GetComponent<Button>();
            dir.onClick.RemoveAllListeners();
            Vector3 pos = posiciones[i];
            dir.onClick.AddListener(() => { prepararInt(pos); });
        }
        elegirDir = StartCoroutine(popUp(P_elegirDirCaja));
    }
    public void cerrarDireccion()
    {
        if (elegirDir != null)
            StopCoroutine(elegirDir);
        P_elegirDirCaja.gameObject.SetActive(false);
    }

   IEnumerator popUp(RectTransform rect)
    {
        rect.localScale = Vector3.zero;
        rect.gameObject.SetActive(true);
        float now = 0;
        float tiempoInv = 1 / tiempoTransicion;
        yield return null;
        while (now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            rect.localScale = Vector3.one * (now * tiempoInv);
            yield return null;
        }
        rect.localScale = Vector3.one;
    }
    public void MostrarControlInteractuar(bool activar)
    {
        if(activar)
        {
            Coroutine_recordarInteractuar = StartCoroutine(popUp(recordarInteractuar));
            recordando = true;
        }
        else
        {
            if(Coroutine_recordarInteractuar != null)
            {
                StopCoroutine(Coroutine_recordarInteractuar);
                recordarInteractuar.gameObject.SetActive(false);
                recordando = false;
            }
        }
    }
   

    public void reintentarNivel()
    {
        if(control.escenaControlador.nivelActual != null &&
            !control.escenaControlador.nivelActual.completado)
        {

            StartCoroutine(reintentar());

        }
    }
    IEnumerator reintentar()
    {
        control.iniciarPantallaCargar();
        yield return null;
        control.escenaControlador.nivelActual.reintentarNivel();
        yield return new WaitForSecondsRealtime(1f);
        control.cargando = false;
    }
}
