using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inicio : Pagina
{
    //---------------elementos para inicializar----------
    public TMPro.TextMeshProUGUI[] Text_textos;
    public RectTransform[] Rect_Opciones;


    //------------------publico-------------------
    //-------------------privada----------
    int tipoFuente = 0;
    int tamanyoFuente = 0;

    void OnEnable()
    {
        if(control != null)
            control.guardarDatoSistema();
    }

    public override void inicializar( Configuracion m)
    {
        base.inicializar( m);
        DatosSistema datos = control.datosSistema;
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
                proporcion = new Vector3(1.25f, 1.25f,1);
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                break;
            case 2:
                proporcion = new Vector3(1.5f, 1.5f,1);
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


    public void salir()
    {
        control.cerrarJuego();
    }
}
