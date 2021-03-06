﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DatosJuego
{
    public int hash;
    public Color colorCamisa,colorFalda;
    public int nivelActual;//nivel que esta el jugador
    public NivelConfig[] niveles;
    public int[] escenaActiva;//escenas que estan cargado
    public Vector3 playerPos;
    public string mapaisala;
    public DatosJuego()
    {
        colorCamisa = new Color(0.5943f,0.0084f,0.0712f,1);
        colorFalda = new Color(0.283f, 0.283f, 0.283f, 1);
        niveles = new NivelConfig[2];
        escenaActiva = new int[5];
        for(int i = 0;i < escenaActiva.Length;i++)
        {
            escenaActiva[i] = 0;
        }
        escenaActiva[0] = 1;
        nivelActual = 0;
        playerPos = new Vector3(298.84f, 11.96f, 44.95f);
        mapaisala = "Mapa/Mapa_Inicial";
    }


    public override int GetHashCode()
    {
        int hashCode = 0;
        hashCode += (int)(colorCamisa.grayscale*256) + (int)(colorFalda.grayscale * 256);
        for(int i = 0;escenaActiva.Length< 5;i++)
        {
            hashCode += escenaActiva[i];
        }
        for(int i = 0; i< niveles.Length;i++)
        {
            if(niveles[i] != null)
            {
                hashCode += niveles[i].GetHashCode();
            }
        }
        hashCode += nivelActual;
        hashCode += mapaisala.GetHashCode() ;
        return hashCode;
    }
    public bool comprobarDatos()
    {
        return hash == GetHashCode();
    }
}
