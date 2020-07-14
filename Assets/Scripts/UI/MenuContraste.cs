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
    TipoContraste colorCambiado ;
    public override void inicializar(Controlador c,Configuracion m)
    {
        base.inicializar(c, m);
        DatosSistema datos = control.datosSistema;
        cambiarFuente(datos.tipoFuente);
        cambiarTamanyo(datos.tamanyoFuente);
        T_Fondo[obtenerIndice(datos.opacidad_fondo)].isOn = true;
        T_Personaje[obtenerIndice(datos.opacidad_personaje)].isOn = true;
        T_Interactivos[obtenerIndice(datos.opacidad_interactivo)].isOn = true;
        actualizarOpacidad();
        colorCambiado = TipoContraste.NADA;
    }
    public int obtenerIndice(float opacidad)
    {
        if (opacidad == 0)
            return 0;
        if (opacidad == 0.25f)
            return 1;
        if (opacidad == 0.5f)
            return 2;
        if (opacidad == 0.75f)
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
    }
    public void cambiarFondoColor(float opacidad)
    {
        control.datosSistema.opacidad_fondo = opacidad;
        obtenerColor(ref control.datosSistema.color_fondo, opacidad);
        actualizarOpacidad();
        control.S_confirmarToggle();
        colorCambiado = colorCambiado | TipoContraste.FONDO;
    }
    public void cambiarFondoColor(Color color)
    {
        control.datosSistema.color_fondo = color;
        obtenerColor(ref control.datosSistema.color_fondo, control.datosSistema.opacidad_fondo);
        actualizarOpacidad();
        colorCambiado = colorCambiado | TipoContraste.FONDO;
    }
    public void cambiarFondoColor()
    {
        control.ElegirColor(posFondo.position, new System.Action<Color>(cambiarFondoColor));
        control.S_confirmarToggle();
    }
    public void cambiarPersonajeColor(float opacidad)
    {
        control.datosSistema.opacidad_personaje = opacidad;
        obtenerColor(ref control.datosSistema.color_personaje, opacidad);
        actualizarOpacidad();
        control.S_confirmarToggle();
        colorCambiado = colorCambiado | TipoContraste.PERSONAJE;
    }
    public void cambiarPersonajeColor(Color color)
    {
        control.datosSistema.color_personaje = color;
        obtenerColor(ref control.datosSistema.color_personaje, control.datosSistema.opacidad_personaje);
        actualizarOpacidad();
        colorCambiado = colorCambiado | TipoContraste.PERSONAJE;
    }
    public void cambiarPersonajeColor()
    {
        control.ElegirColor(posPersonaje.position, new System.Action<Color>(cambiarPersonajeColor));
        control.S_confirmarToggle();
    }
    public void cambiarInteractivoColor(float opacidad)
    {
        control.datosSistema.opacidad_interactivo = opacidad;
        obtenerColor(ref control.datosSistema.color_interactivo, opacidad);
        actualizarOpacidad();
        control.S_confirmarToggle();
        colorCambiado = colorCambiado | TipoContraste.INTERACTIVO;
    }
    public void cambiarInteractivoColor(Color color)
    {
        control.datosSistema.color_interactivo = color;
        obtenerColor(ref control.datosSistema.color_interactivo, control.datosSistema.opacidad_interactivo);
        actualizarOpacidad();
        colorCambiado = colorCambiado | TipoContraste.INTERACTIVO;
    }
    public void cambiarInteractivoColor()
    {
        control.ElegirColor(posInteractivo.position, new System.Action<Color>(cambiarInteractivoColor));
        control.S_confirmarToggle();
    }
    
    void obtenerColor(ref Color color,float opacidad)
    {
        float h, s, v;
        Color.RGBToHSV(color,out h,out s,out v);
        color = Color.HSVToRGB(h, opacidad, 1);
    }

    void OnDisable()
    {
        if(colorCambiado != TipoContraste.NADA)
        {
            control.cambiarContraste(colorCambiado);
        }
    }
}
