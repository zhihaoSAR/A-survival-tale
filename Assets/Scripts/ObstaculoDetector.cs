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
        chocado = true;
    }
    void OnTriggerExit(Collider other)
    {
        chocado = false;
    }
}
