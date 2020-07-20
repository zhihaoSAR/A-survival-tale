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
        if (abierto)
            return;
        animator.SetTrigger("abrir");
    }
}
