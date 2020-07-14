using System.Collections;
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
    public DatosJuego()
    {
        colorCamisa = new Color(0.5943f,0.0084f,0.0712f,1);
        colorFalda = new Color(0.283f, 0.283f, 0.283f, 1);
        niveles = new NivelConfig[2];
        escenaActiva = new int[5];
        nivelActual = 0;
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
        return hashCode;
    }
    public bool comprobarDatos()
    {
        return hash == GetHashCode();
    }
}
