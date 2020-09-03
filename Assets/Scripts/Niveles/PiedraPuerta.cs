using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiedraPuerta : Puerta
{
    public Animator animator;
    [HideInInspector]
    public bool abierto = false;
    public override void Abrir(bool abrir)
    {
        if (abierto || !abrir)
            return;
        animator.SetTrigger("abrir");
    }
    public void configEstado(bool abierto)
    {
        this.abierto = abierto;
        if (abierto)
        {
            animator.enabled = false;
        }
        else
        {
            animator.Rebind();
        }
    }

}
