using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
public class Caja : Interactuable
{
    [SerializeField]
    Transform arriba, derecha, abajo, izquierda;
    [SerializeField]
    bool asaArriba, asaDerecha, asaAbajo, asaIzquierda;
    [HideInInspector]
    public Vector3 dir;
    [HideInInspector]
    public bool conAsa;
    Vector3 posicionInt = Vector3.zero;
    public ObstaculoDetector detector;
    Action finalizado;
    public NavMeshObstacle obstaculo;

    public Rigidbody rb;

    enum DetectorPos { ARRIBA =0, DERECHA = 1, ABAJO = 2 ,IZQUIERDA =3, SINVALOR}
    DetectorPos detectorPos = DetectorPos.SINVALOR;

    public override void finPreparar()
    {
        
        obtenerPosicion(player.transform);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        detector.gameObject.SetActive(true);
        player.prepararConCaja();
        activaDetector();

    }
    void activaDetector()
    {
        detector.colisiones[(int)detectorPos].enabled = true;
    }
    void desactivaDetector()
    {
        if(detectorPos != DetectorPos.SINVALOR )
        {
            detector.colisiones[(int)detectorPos].enabled = false;
            detectorPos = DetectorPos.SINVALOR;
        }
        
    }
    public override void finInteractuar()
    {
        posicionInt = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        desactivaDetector();
        detector.chocado = false;
        detector.gameObject.SetActive(false);
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
        return obtenerPosicion(posicion.position);
    }
    public Vector3 obtenerPosicion(Vector3 posicion)
    {
        
        Vector3 dst = arriba.position;
        float minDis = (posicion - arriba.position).sqrMagnitude;
        dir = transform.forward * -1;
        conAsa = asaArriba;
        detectorPos = DetectorPos.ABAJO;
        if ((posicion - derecha.position).sqrMagnitude < minDis)
        {
            dir = transform.right * -1;
            conAsa = asaDerecha;
            minDis = (posicion - derecha.position).sqrMagnitude;
            dst = derecha.position;
            detectorPos = DetectorPos.IZQUIERDA;
        }
        if ((posicion - abajo.position).sqrMagnitude < minDis)
        {
            dir = transform.forward;
            conAsa = asaAbajo;
            minDis = (posicion - abajo.position).sqrMagnitude;
            dst = abajo.position;
            detectorPos = DetectorPos.ARRIBA;
        }
        if ((posicion - izquierda.position).sqrMagnitude < minDis)
        {
            dir = transform.right;
            conAsa = asaIzquierda;
            minDis = (posicion- izquierda.position).sqrMagnitude;
            dst = izquierda.position;
            detectorPos = DetectorPos.DERECHA;
        }
        posicionInt = transform.InverseTransformPoint(dst);
        return dst;
    }
    public void Finalizado()
    {
        desactivarCaja();
        finalizado();
    }

    public void desactivarCaja()
    {
        rb.detectCollisions = false;
        desactivaDetector();
        enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        obstaculo.enabled = false;
        detector.chocado = false;
    }

    public void caer(Action fin)
    {
        finalizado = fin;
        player.CancelarInteractuarConCaja();
        GetComponent<Animator>().SetTrigger("caer");
    }
    /*
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
    }*/
    public void reseteaPos()
    {
        posicionInt = Vector3.zero;

    }
}
