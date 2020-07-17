using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
public class DetectorCajaCaer : AreaTrigger
{

    public NavMeshObstacle obstaculo;
    public override void Activar()
    {
        contador--;
        if (contador == 0)
        {
            for(int i = 1;i<triggers.Length;i++)
            {
                if(triggers[i-1].objeto != triggers[i].objeto)
                {
                    return;
                }
            }
            Caja caja;
            if(!triggers[0].objeto.TryGetComponent<Caja>(out caja))
            {
                return;
            }
            caja.caer(new Action(DesactivarObstaculo));
            if (unaVez)
            {
                gameObject.SetActive(false);
            }
        }
    }
    void DesactivarObstaculo()
    {
        obstaculo.enabled = false;
        gameObject.SetActive(false);
    }
    public override void Desactivar()
    {
        contador++;
        if (contador == 1)
        {
            if (unaVez)
            {
                gameObject.SetActive(false);
            }
        }

    }

}