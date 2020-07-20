using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coco : Interactuable
{


    public Rigidbody rb;
    public SphereCollider detector;

    public override void finPreparar()
    {
        player.estado = Player.Estado.PARADOCONCOCO;
        player.estadoActual = player.estadoParadoConCoco;
        player.cogerCocoAnim();
        
        transform.parent = player.posCogerCoco.transform;
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
    public void interactuarConAve(Ave ave)
    {
        transform.parent = ave.posicionCoco.transform;
        rb.detectCollisions = false;
        rb.isKinematic = true;
        transform.position = ave.posicionCoco.position;
        interactuable = false;
        detector.enabled = false;

    }
    public void finInteractuarConAve()
    {
        finInteractuar();
        interactuable = true;
        detector.enabled = true;
    }
}
