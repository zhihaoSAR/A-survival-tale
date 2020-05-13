using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Visualizacion : Pagina
{
    //---------------elementos para inicializar----------
    public TextMeshProUGUI Text_tituloGrande;
    public TextMeshProUGUI[] Text_tituloOpciones;
    public TextMeshProUGUI[] Text_botones;
    public RectTransform[] Rect_botones;
    public Image[] I_cursoresPrev;
    public Toggle[] T_PantallaCompleta;
    public Toggle[] T_TamanyoLetra;
    public Toggle[] T_Fuente;
    public Toggle[] T_CursorTipo;
    public Toggle[] T_CursorTamanyo;
    //------------------publico-------------------
    //[HideInInspector]
    public Controlador control;
    

    //--------------------privada------------------
    int tipoFuente = 0;
    int tamanyoFuente = 1;
    int tipoCursor = 0;
    int tamanyoCursor = 0;
    
    public override void inicializar(Controlador c)
    {
        control = c;
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
                Text_tituloGrande.fontSize = 26;
                proporcion = new Vector2(0.8f, 0.8f);
                foreach(RectTransform transform in Rect_botones)
                {
                    transform.localScale = proporcion;
                }
                foreach(TextMeshProUGUI text in Text_tituloOpciones)
                {
                    text.fontSize = 23;
                }
                break;
            case 1:
                Text_tituloGrande.fontSize = 32;
                proporcion = Vector2.one;
                foreach (RectTransform transform in Rect_botones)
                {
                    transform.localScale = proporcion;
                }
                foreach (TextMeshProUGUI text in Text_tituloOpciones)
                {
                    text.fontSize = 28;
                }
                break;
            case 2:
                Text_tituloGrande.fontSize = 39;
                proporcion = new Vector2(1.2f,1.2f);
                foreach (RectTransform transform in Rect_botones)
                {
                    transform.localScale = proporcion;
                }
                foreach (TextMeshProUGUI text in Text_tituloOpciones)
                {
                    text.fontSize = 34;
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
        TMP_FontAsset fuente;
        if(tipo == 0)
        {
            fuente = Resources.Load<TMP_FontAsset>("TCM");
        }
        else
        {
            fuente = Resources.Load<TMP_FontAsset>("TCB");
        }
        Text_tituloGrande.font = fuente;
        foreach(TextMeshProUGUI text in Text_tituloOpciones)
        {
            text.font = fuente;
        }
        foreach (TextMeshProUGUI text in Text_botones)
        {
            text.font = fuente;
        }
        tipoFuente = tipo;
        control.datosSistema.tipoFuente = tipo;
            
    }

    public void cambiarCursorTamanyo(int tamanyo)
    {
        if (tamanyoCursor == tamanyo)
            return;
        control.cambiarCursor(tipoCursor, tamanyo);
        tamanyoCursor = tamanyo;
        control.datosSistema.tamanyoCursor = tamanyo;
    }
    public void cambiarCursorTipo(int tipo)
    {
        if (tipoCursor == tipo)
            return;
        control.cambiarCursor(tipo, tamanyoCursor);
        tipoCursor = tipo;
        control.datosSistema.tipoCursor = tipo;
        configurarCursorImagen(tipo);
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
    }

}
