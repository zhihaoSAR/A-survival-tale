using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Interactuable : MonoBehaviour
{
    [SerializeField]
    float distanciaInteractuable;
    public enum TipoInteractuable{CAJA,COCO,AVE }
    public bool interactuable = true;
    public TipoInteractuable tipo;
    [HideInInspector]
    public Player player;
    void Start()
    {
        player = Controlador.control.player;
    }
    public virtual Vector3 obtenerPosicion(Transform posicion)
    {
        Vector3 dir = posicion.position - transform.position;
        dir.y = 0;
        return transform.position + dir.normalized*distanciaInteractuable;
    }
    public abstract void finPreparar();
    public abstract void finInteractuar();
}
