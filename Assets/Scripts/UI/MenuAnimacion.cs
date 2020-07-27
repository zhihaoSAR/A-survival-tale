using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimacion : Pagina
{
    //---------------elementos para inicializar----------
    public TextMeshProUGUI[] Text_textos;
    public RectTransform[] Rect_Opciones;
    public RectTransform[] Rect_fondos;
    public Toggle[] T_AnimacionNPC;
    public Toggle[] T_AnimacionPeligro;
    public Toggle[] T_AnimacionDecoracion;
    //------------------publico-------------------
    //-------------------privada----------
    int tipoFuente = 0;
    int tamanyoFuente = 0;

    public override void inicializar( Configuracion m)
    {
        base.inicializar( m);
        DatosSistema datos = control.datosSistema;
        cambiarFuente(datos.tipoFuente);
        cambiarTamanyo(datos.tamanyoFuente);
        T_AnimacionNPC[datos.activarNpcAnim].isOn = true;
        T_AnimacionPeligro[datos.activarPeligroAnim].isOn = true;
        T_AnimacionDecoracion[datos.activarDecoracionAnim].isOn = true;
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
                foreach (RectTransform transform in Rect_fondos)
                {
                    transform.localScale = new Vector3(proporcion.x, 1,1);
                }
                break;
            case 1:
                proporcion = new Vector3(1.25f, 1.25f,1);
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                foreach (RectTransform transform in Rect_fondos)
                {
                    transform.localScale = new Vector3(proporcion.x, 1,1);
                }
                break;
            case 2:
                proporcion = new Vector3(1.5f, 1.5f,1f);
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                foreach (RectTransform transform in Rect_fondos)
                {
                    transform.localScale = new Vector3(proporcion.x, 1,1);
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
    // 0:desactivar 1:activar
    public void activarNpcAnim(int activar)
    {
        control.datosSistema.activarNpcAnim = activar;
        control.S_confirmarToggle();
    }
    // 0:desactivar 1:activar
    public void activarPeligroAnim(int activar)
    {
        control.datosSistema.activarPeligroAnim = activar;
        control.S_confirmarToggle();
    }
    // 0:desactivar 1:activar
    public void activarDecoracionAnim(int activar)
    {
        control.datosSistema.activarDecoracionAnim = activar;
        control.ConfEfectoAnimacion();
        control.S_confirmarToggle();
    }
}
