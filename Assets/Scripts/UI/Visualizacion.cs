using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Visualizacion : Pagina
{
    //---------------elementos para inicializar----------
    public TextMeshProUGUI[] Text_textos;
    public RectTransform[] Rect_Opciones;
    public RectTransform[] Rect_fondos;
    public Image[] I_cursoresPrev;
    public Toggle[] T_PantallaCompleta;
    public Toggle[] T_TamanyoLetra;
    public Toggle[] T_Fuente;
    public Toggle[] T_CursorTipo;
    public Toggle[] T_CursorTamanyo;
    //------------------publico-------------------
    

    //--------------------privada------------------
    int tipoFuente = 0;
    int tamanyoFuente = 0;
    int tipoCursor = 0;
    int tamanyoCursor = 0;

    public override void inicializar(Controlador c, Configuracion m)
    {
        base.inicializar(c, m);
        DatosSistema datos = control.datosSistema;
        cambiarFuente(datos.tipoFuente);
        cambiarTamanyo(datos.tamanyoFuente);
        T_PantallaCompleta[datos.pantallaCompleta].isOn = true;
        T_TamanyoLetra[datos.tamanyoFuente].isOn = true;
        T_Fuente[datos.tipoFuente].isOn = true;
        T_CursorTipo[datos.tipoCursor].isOn = true;
        T_CursorTamanyo[datos.tamanyoCursor].isOn = true;

    }
    public void cambiarTamanyo(int tamanyo)
    {
        if (tamanyoFuente == tamanyo)
            return;
        Vector2 proporcion;
        switch(tamanyo)
        {
            case 0:
                proporcion = Vector2.one;
                foreach(RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                foreach (RectTransform transform in Rect_fondos)
                {
                    transform.localScale =new Vector2(proporcion.x,1);
                }
                break;
            case 1:
                proporcion = new Vector2(1.25f,1.25f);
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                foreach (RectTransform transform in Rect_fondos)
                {
                    transform.localScale = new Vector2(proporcion.x, 1);
                }
                break;
            case 2:
                proporcion = new Vector2(1.5f,1.5f);
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                foreach (RectTransform transform in Rect_fondos)
                {
                    transform.localScale = new Vector2(proporcion.x, 1);
                }
                break;
        }
        tamanyoFuente = tamanyo;
        control.datosSistema.tamanyoFuente = tamanyo;
    }
    public void cambiarTamanyoConSonido(int tamanyo)
    {
        cambiarTamanyo(tamanyo);
        control.S_confirmarToggle();
    }
    //0:tcm 1:tcb
    public void cambiarFuente(int tipo)
    {
        if (tipoFuente == tipo)
            return;
        TMP_FontAsset fuente =control.getFont(tipo);
        foreach(TextMeshProUGUI text in Text_textos)
        {
            text.font = fuente;
        }
        tipoFuente = tipo;
        control.datosSistema.tipoFuente = tipo;
            
    }

    public void cambiarFuenteConSonido(int tipo)
    {
        control.S_confirmarToggle();
        cambiarFuente(tipo);
    }

    public void cambiarCursorTamanyo(int tamanyo)
    {
        if (tamanyoCursor == tamanyo)
            return;
        control.cambiarCursor(tipoCursor, tamanyo);
        tamanyoCursor = tamanyo;
        control.datosSistema.tamanyoCursor = tamanyo;
        control.S_confirmarToggle();
    }
    public void cambiarCursorTipo(int tipo)
    {
        if (tipoCursor == tipo)
            return;
        control.cambiarCursor(tipo, tamanyoCursor);
        tipoCursor = tipo;
        control.datosSistema.tipoCursor = tipo;
        configurarCursorImagen(tipo);
        control.S_confirmarToggle();
    }
    void configurarCursorImagen(int tipo)
    {
        string prefijo;
        if (tipo == 0)
        {
            prefijo = "cursorPrev_";
        }
        else
        {
            prefijo = "cursorEspPrev_";
        }
        I_cursoresPrev[0].sprite = Resources.Load<Sprite>(prefijo + "peq");
        I_cursoresPrev[1].sprite = Resources.Load<Sprite>(prefijo + "med");
        I_cursoresPrev[2].sprite = Resources.Load<Sprite>(prefijo + "gran");

    }

    //1:ventana 0:completa
    public void pantallaCompleta(int modo)
    {
        control.PantallaCompleta(modo);
        control.S_confirmarToggle();
    }

}
