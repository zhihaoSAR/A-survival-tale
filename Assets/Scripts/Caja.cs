using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Caja : Interactuable
{
    [SerializeField]
    Transform arriba, derecha, abajo, izquierda;
    [SerializeField]
    bool asaArriba, asaDerecha, asaAbajo, asaIzquierda;
    public Vector3 dir;
    [HideInInspector]
    public bool conAsa;

    public Rigidbody rb;
    
    public override void finPreparar()
    {
        player.estadoActual = player.estadoParadoConCaja;
        obtenerPosicion(player.transform);
        transform.parent = player.transform;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

    }
    public override void finInteractuar()
    {
        transform.parent = null;
        rb.constraints = RigidbodyConstraints.FreezeAll;
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
        dir = transform.forward*-1;
        conAsa = asaArriba;
        if((posicion.position - derecha.position).sqrMagnitude < minDis)
        {
            dir = transform.right*-1;
            conAsa = asaDerecha;
            minDis = (posicion.position - derecha.position).sqrMagnitude;
            dst = derecha.position;
        }
        if ((posicion.position - abajo.position).sqrMagnitude < minDis)
        {
            dir = transform.forward;
            conAsa = asaAbajo;
            minDis = (posicion.position - abajo.position).sqrMagnitude;
            dst = abajo.position;
        }
        if ((posicion.position - izquierda.position).sqrMagnitude < minDis)
        {
            dir = transform.right;
            conAsa = asaIzquierda;
            minDis = (posicion.position - izquierda.position).sqrMagnitude;
            dst = izquierda.position;
        }
        return dst;
    }

}
