using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TipoVolumen {AMBIENTE=0,PELIGRO=1,INTERACCION=2,INTERFAZ=3,PASO=4 }

public class Volumen : Pagina
{
    //---------------elementos para inicializar----------
    public TextMeshProUGUI[] Text_textos;
    public RectTransform[] Rect_Opciones;
    public RectTransform[] Rect_fondos;
    public Toggle[] T_ambiente,T_peligro,T_interaccion,T_interfaz,T_paso;
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
        T_ambiente[(int)(datos.ambienteVol/25)].isOn = true;
        T_peligro[(int)(datos.peligroVol / 25)].isOn = true;
        T_interaccion[(int)(datos.interaccionVol / 25)].isOn = true;
        T_interfaz[(int)(datos.interfazVol / 25)].isOn = true;
        T_paso[(int)(datos.pasoVol / 25)].isOn = true;
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
    //0:ambiente 1: peligro 2:interaccion 3:interfaz 4:paso
    //formato "tipo,vol"s
    public void ModificarVolumen(string tipoVol)
    {
        string[] valores = tipoVol.Split(',');
        Debug.Log("soy volumen tipo es" + valores[0]+"     "+ valores[1]);
        int tipo = int.Parse(valores[0]);
        float vol = float.Parse(valores[1]);
        control.ModificarVolumen((TipoVolumen)tipo,vol);
        control.S_confirmarToggle();
    }
}
