using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Nivel : MonoBehaviour
{
    public abstract NivelConfig generarConfig();
    public int id;
    [HideInInspector]
    public bool completado;

    public virtual void Start()
    {
       if(Controlador.control.escenaControlador.numNivelActual == id)
        {
            Controlador.control.escenaControlador.nivelActual = this;
        }
       if(Controlador.control.datosJuego.niveles[id]!=null)
        {
            cargarConf(Controlador.control.datosJuego.niveles[id]);
        }
    }
    public abstract void cargarConf(NivelConfig conf);
    public abstract void reintentarNivel();
    public virtual void completadoNivel()
    {
        completado = true;
    }

}
