using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Caja : Interactuable
{
    [SerializeField]
    Transform arriba, derecha, abajo, izquierda;
    
    public override void setEstado()
    {
        player.estadoActual = player.estadoParadoConCaja;
    }
    public Vector3[] obtenerPosiciones()
    {
        Vector3[] posiciones = {arriba.position,derecha.position,abajo.position,izquierda.position};
        return posiciones;
    }
    public override Vector3 obtenerPosicion(Transform posicion)
    {
        Vector3 dst = arriba.position;
        float minDis = (posicion.position-arriba.position).sqrMagnitude;
        Vector3[] posiciones = obtenerPosiciones();
        for (int i = 1;i < 4;i++)
        {
            if((posiciones[i] -posicion.position).sqrMagnitude < minDis)
            {
                dst = posiciones[i];
                minDis = (posiciones[i] - posicion.position).sqrMagnitude;
            }
        }


        return dst;
    }

}
