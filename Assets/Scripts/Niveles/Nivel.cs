using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Nivel : MonoBehaviour
{
    public abstract NivelConfig generarConfig(string nivel);
    public int id;

    void Start()
    {
       if(Controlador.control.escenaControlador.numNivelActual == id)
        {
            Controlador.control.escenaControlador.nivelActual = this;
        }
    }
    
}
