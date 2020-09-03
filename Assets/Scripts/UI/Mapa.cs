using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Mapa : MonoBehaviour
{
    Controlador control;
    public GameObject[] navegables;
    //---------------elementos para inicializar----------
    public TextMeshProUGUI[] Text_textos;
    public RectTransform[] Rect_Opciones;
    public Button reintentar;
    public Image mapaisla;
    //------------------publico-------------------
    public RectTransform[] posiciones;
    public RectTransform indicador;
    //-------------------privada----------
    int tipoFuente = 0;
    int tamanyoFuente = 0;

    public void Start()
    {
        control = Controlador.control;
        Debug.Log("ahahsihdi");
        Debug.Log(control);
        actualizarMapa();
        gameObject.SetActive(false);
    }
    public void actualizarMapa()
    {
        mapaisla.sprite = Resources.Load<Sprite>(control.datosJuego.mapaisala);
    }
    void actualizarPos()
    {
        int nivelId = control.escenaControlador.numNivelActual;
        if(nivelId == 0)
        {
            indicador.position = posiciones[0].position;
            return;
        }
        if(nivelId == 1)
        {
            indicador.position = posiciones[1].position;
        }
    }
    public void inicializar()
    {
        DatosSistema datos = control.datosSistema;
        actualizarPos();
        reintentar.enabled = (control.escenaControlador.nivelActual != null &&
            !control.escenaControlador.nivelActual.completado);
        cambiarFuente(datos.tipoFuente);
        cambiarTamanyo(datos.tamanyoFuente);
    }
    public void cambiarTamanyo(int tamanyo)
    {
        if (tamanyoFuente == tamanyo)
            return;
        Vector3 proporcion;
        switch (tamanyo)
        {
            case 0:
                proporcion = Vector3.one;
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                break;
            case 1:
                proporcion = new Vector3(1.25f, 1.25f, 1);
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                break;
            case 2:
                proporcion = new Vector3(1.5f, 1.5f, 1f);
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                break;
        }
        tamanyoFuente = tamanyo;
        control.datosSistema.tamanyoFuente = tamanyo;
    }
    //0:tcm 1:tcb
    public void cambiarFuente(int tipo)
    {
        if (tipoFuente == tipo)
            return;
        TMP_FontAsset fuente = control.getFont(tipo);
        foreach (TextMeshProUGUI text in Text_textos)
        {
            text.font = fuente;
        }
        tipoFuente = tipo;
        control.datosSistema.tipoFuente = tipo;

    }

}
