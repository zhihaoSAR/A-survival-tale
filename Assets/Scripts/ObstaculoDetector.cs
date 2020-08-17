using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoDetector : MonoBehaviour
{
    public Collider[] colisiones;
    [HideInInspector]
    public bool chocado = false;

    void OnTriggerStay(Collider collider)
    {
        

        if(collider.isTrigger)
        {
            return;
        }
        chocado = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        chocado = false;
    }
}
