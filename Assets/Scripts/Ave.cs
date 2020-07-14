using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ave : Interactuable
{
    public Rigidbody rb;
    public bool chocado = false;
    bool tenerObjeto = false;
    Coco coco;
    [HideInInspector]
    public GameObject objetoChocado;
    public Transform posicionCoco;
    public SphereCollider detector;
    public Animator animator;

    public override void finInteractuar()
    {
        transform.parent = null;
        rb.isKinematic = false;
        //rb.detectCollisions = true;

    }

    public override void finPreparar()
    {
        player.estado = Player.Estado.PARADOCONAVE;
        player.estadoActual = player.estadoParadoConAve;

        transform.rotation = player.transform.rotation;
        transform.parent = player.transform;
        //rb.detectCollisions = false;
        rb.isKinematic = true;
        
        transform.position = player.posCogerAve.position;
        
    }

    public void EstirarAve()
    {
        detector.enabled = true;
        animator.SetBool("alargar", true);
        player.estirarAve();
        StartCoroutine(alargar());
    }
    IEnumerator alargar()
    {
        chocado = false;
        bool start = false;
        
        yield return null;
        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
                start = true;
            if (chocado || (start && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1))
            {
                animator.SetBool("alargar", false);
                detector.enabled = false;
                if(tenerObjeto)
                {
                    coco.finInteractuarConAve();
                    coco.rb.AddForce(transform.forward * 15, ForceMode.Impulse);
                    coco = null;
                    tenerObjeto = false;
                    break;
                }
                if(chocado)
                {
                    if(objetoChocado.TryGetComponent<Coco>(out coco))
                    {
                        tenerObjeto = true;
                        coco.interactuarConAve(this);
                        
                    }
                }
                break;
            }
            yield return null;
        }
        player.acabadoEstirar = true;
    }
}
