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
    Vector3 posicionInt = Vector3.zero;

    public Rigidbody rb;
    
    public override void finPreparar()
    {
        player.estado = Player.Estado.PARADOCONCAJA;
        player.estadoActual = player.estadoParadoConCaja;
        obtenerPosicion(player.transform);
        transform.parent = player.transform;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

    }
    public override void finInteractuar()
    {
        transform.parent = null;
        posicionInt = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    public Vector3[] obtenerPosiciones()
    {
        Vector3[] posiciones = {arriba.position,derecha.position,abajo.position,izquierda.position};
        return posiciones;
    }
    public override Vector3 obtenerPosicion(Transform posicion)
    {
        if (!posicionInt.Equals(Vector3.zero))
            return transform.TransformPoint(posicionInt);
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
        posicionInt = transform.InverseTransformPoint(dst);
        return dst;
    }
    public void asigPosInt(Vector3 pos)
    {
        Vector3 dst = arriba.position;
        dir = transform.forward * -1;
        conAsa = asaArriba;
        if (pos.Equals(derecha.position))
        {
            dir = transform.right * -1;
            conAsa = asaDerecha;
            dst = derecha.position;
        }
        if (pos.Equals(abajo.position))
        {
            dir = transform.forward;
            conAsa = asaAbajo;
            dst = abajo.position;
        }
        if (pos.Equals(izquierda.position))
        {
            dir = transform.right;
            conAsa = asaIzquierda;
            dst = izquierda.position;
        }
        posicionInt = transform.InverseTransformPoint(dst);
    }
}
