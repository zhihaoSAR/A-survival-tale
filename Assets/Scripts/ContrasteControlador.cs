using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum TipoContraste
{
    NADA = 0,
    FONDO = 1,
    PERSONAJE = 2,
    INTERACTIVO = 4
}

public class ContrasteControlador:MonoBehaviour
{
    public ColorDefecto colorDefecto;

    public Material[] fondos;
    public Material[] aguas;
    public Material[] fuegos;
    public Material[] jugador;
    public Material[] interactuables;

    public void cambiarContrasteFondo(Color color)
    {
        for(int i = 0; i< fondos.Length;i++)
        {
            fondos[i].color = color;
        }
        for (int i = 0; i < aguas.Length; i++)
        {
            aguas[i].SetColor("_BaseColor",color);
        }
        for (int i = 0; i < fuegos.Length; i++)
        {
            aguas[i].SetColor("_TintColor", color);
        }
    }
    public void cambiarContrastePesonaje(Color color)
    {
        for (int i = 0; i < jugador.Length; i++)
        {
            jugador[i].color = color;
        }
    }
    public void cambiarContrasteInteractuable(Color color)
    {
        for (int i = 0; i < interactuables.Length; i++)
        {
            interactuables[i].color = color;
        }
    }
    public void cambiarAlDefectoFondo()
    {
        for (int i = 0; i < fondos.Length; i++)
        {
            fondos[i].color = colorDefecto.fondos[i];
        }
        for (int i = 0; i < aguas.Length; i++)
        {
            aguas[i].SetColor("_BaseColor", colorDefecto.aguas[i]);
        }
        for (int i = 0; i < fuegos.Length; i++)
        {
            fuegos[i].SetColor("_TintColor", colorDefecto.fuegos[i]);
        }
    }
    public void cambiarAlDefectoPersonaje(DatosJuego datos)
    {
        jugador[0].color = datos.colorCamisa;
        jugador[1].color = datos.colorFalda;
        for (int i = 2; i < jugador.Length; i++)
        {
            jugador[i].color = colorDefecto.jugador[i];
        }
    }
    public void cambiarAlDefectoInteractuable()
    {
        for (int i = 0; i < interactuables.Length; i++)
        {
            interactuables[i].color = colorDefecto.interactuables[i];
            TerrainData data;
        }
    }
#if UNITY_EDITOR
    public void volverAlColorDefecto()
    {
        cambiarAlDefectoFondo();
        cambiarAlDefectoInteractuable();
        jugador[0].color = new Color(0.5943f, 0.0084f, 0.0712f, 1);
        jugador[1].color = new Color(0.283f, 0.283f, 0.283f, 1);
        for (int i = 2; i < jugador.Length; i++)
        {
            jugador[i].color = colorDefecto.jugador[i];
        }
    }
#endif

}
