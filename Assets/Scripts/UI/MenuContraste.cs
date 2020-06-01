using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuContraste : Pagina
{
    //---------------elementos para inicializar----------
    public TextMeshProUGUI[] Text_textos;
    public RectTransform[] Rect_Opciones;
    public RectTransform[] Rect_fondos;
    public Toggle[] T_Fondo;
    public Toggle[] T_Personaje;
    public Toggle[] T_Interactivos;
    public Image C_Fondo;
    public Image C_Personaje;
    public Image C_Interactivo;
    public RectTransform posFondo, posPersonaje, posInteractivo;
    

    //------------------publico-------------------
    //-------------------privada----------
    int tipoFuente = 0;
    int tamanyoFuente = 0;

    public override void inicializar(Controlador c,Configuracion m)
    {
        base.inicializar(c, m);
        DatosSistema datos = control.datosSistema;
        cambiarFuente(datos.tipoFuente);
        cambiarTamanyo(datos.tamanyoFuente);
        T_Fondo[obtenerIndice(datos.color_fondo.a)].isOn = true;
        T_Personaje[obtenerIndice(datos.color_personaje.a)].isOn = true;
        T_Interactivos[obtenerIndice(datos.color_interactivo.a)].isOn = true;
        actualizarOpacidad();
    }
    public int obtenerIndice(float opacidad)
    {
        if (opacidad == 0)
            return 0;
        if (opacidad < 0.26f)
            return 1;
        if (opacidad < 0.51f)
            return 2;
        if (opacidad < 0.76f)
            return 3;
        return 4;
    }
    public void cambiarTamanyo(int tamanyo)
    {
        if (tamanyoFuente == tamanyo)
            return;
        Vector2 proporcion;
        switch (tamanyo)
        {
            case 0:
                proporcion = Vector2.one;
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                foreach (RectTransform transform in Rect_fondos)
                {
                    transform.localScale = new Vector2(proporcion.x, 1);
                }
                break;
            case 1:
                proporcion = new Vector2(1.25f, 1.25f);
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
                proporcion = new Vector2(1.5f, 1.5f);
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
    public void actualizarOpacidad()
    {
        DatosSistema datos = control.datosSistema;
        C_Fondo.color = datos.color_fondo;
        C_Interactivo.color = datos.color_interactivo;
        C_Personaje.color = datos.color_personaje;
        control.S_confirmarToggle();
    }
    public void cambiarFondoColor(float opacidad)
    {
        control.datosSistema.color_fondo.a = opacidad;
        actualizarOpacidad();
        control.S_confirmarToggle();
    }
    public void cambiarFondoColor(Color color)
    {
        color.a = control.datosSistema.color_fondo.a;
        control.datosSistema.color_fondo = color;
        actualizarOpacidad();
    }
    public void cambiarFondoColor()
    {
        control.ElegirColor(posFondo.position, new System.Action<Color>(cambiarFondoColor));
        control.S_confirmarToggle();
    }
    public void cambiarPersonajeColor(float opacidad)
    {
        control.datosSistema.color_personaje.a = opacidad;
        actualizarOpacidad();
    }
    public void cambiarPersonajeColor(Color color)
    {
        color.a = control.datosSistema.color_personaje.a;
        control.datosSistema.color_personaje = color;
        actualizarOpacidad();
    }
    public void cambiarPersonajeColor()
    {
        control.ElegirColor(posPersonaje.position, new System.Action<Color>(cambiarPersonajeColor));
        control.S_confirmarToggle();
    }
    public void cambiarInteractivoColor(float opacidad)
    {
        control.datosSistema.color_interactivo.a = opacidad;
        actualizarOpacidad();
        control.S_confirmarToggle();
    }
    public void cambiarInteractivoColor(Color color)
    {
        color.a = control.datosSistema.color_interactivo.a;
        control.datosSistema.color_interactivo = color;
        actualizarOpacidad();
    }
    public void cambiarInteractivoColor()
    {
        control.ElegirColor(posInteractivo.position, new System.Action<Color>(cambiarInteractivoColor));
        control.S_confirmarToggle();
    }
    
    
        
}
