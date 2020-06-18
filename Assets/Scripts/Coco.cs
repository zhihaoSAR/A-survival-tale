using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coco : Interactuable
{


    public Rigidbody rb;

    public override void finPreparar()
    {
        player.estado = Player.Estado.PARADOCONCOCO;
        player.estadoActual = player.estadoParadoConCoco;
        
        transform.parent = player.transform;
        rb.detectCollisions = false;
        rb.isKinematic = true;
        transform.position = player.posCogerCoco.position;
        
    }
    public override void finInteractuar()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.detectCollisions = true;
    }

}
