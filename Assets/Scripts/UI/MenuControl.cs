using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : Pagina
{
    //---------------elementos para inicializar----------
    public TextMeshProUGUI[] Text_textos;
    public RectTransform[] Rect_Opciones;
    public RectTransform[] Rect_fondos;
    public Toggle[] T_Controles;
    public Toggle[] T_TiempoRegistro;
    public Toggle[] T_Velocidad;

    //------------------publico-------------------
    //-------------------privada----------
    int tipoFuente = 0;
    int tamanyoFuente = 0;
    public Pagina remapeoWASD, remapeo2B;

    public override void inicializar( Configuracion m)
    {
        base.inicializar( m);
        DatosSistema datos = control.datosSistema;
        cambiarFuente(datos.tipoFuente);
        cambiarTamanyo(datos.tamanyoFuente);
        T_Controles[datos.tipoControl].isOn = true;
        T_Velocidad[datos.velocidad].isOn = true;
        int ind = 0;
        if(datos.inputTime > 0.5f)
        {
            ind = 2;
        }else if(datos.inputTime > 0)
        {
            ind = 1;
        }
        T_TiempoRegistro[ind].isOn = true;
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
                proporcion = new Vector3(1.25f, 1.25f,1f);
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
                proporcion = new Vector3(1.5f, 1.5f,1);
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
    // 1:WASD 2:RATON 3:2BOTON
    public void CambiarControl(int tipo)
    {
        control.CambiarControl(tipo);
        control.S_confirmarToggle();
    }
    public void CambiarVelocidad(int vel)
    {
        control.datosSistema.velocidad = vel;
        if(control.player != null)
        {
            control.player.actualizarVelocidad();
        }
        control.S_confirmarToggle();
    }
    public void CambiarTiempoRegistro(float tiempo)
    {
        control.datosSistema.inputTime = tiempo;
        control.S_confirmarToggle();
    }
    public void remapeo()
    {
        if (control.datosSistema.tipoControl == 2)
        {
            menu.abrirPagina(remapeo2B);
        }
        else
        {
            menu.abrirPagina(remapeoWASD);
        }
    }
}
