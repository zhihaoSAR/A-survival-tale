using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsignarNivel : MonoBehaviour
{
    public Nivel nivel;
    public int id;
    void OnTriggerEnter(Collider collider)
    {
        if(nivel != null)
        {
            Controlador.control.escenaControlador.asignarNivel(nivel, id);
        }
        else
        {
            Controlador.control.escenaControlador.asignarNivel(null, id);
        }
    }

}
