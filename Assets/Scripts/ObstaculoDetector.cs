using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoDetector : MonoBehaviour
{
    public Collider[] colisiones;
    [HideInInspector]
    public bool chocado = false;

    void OnTriggerEnter(Collider collider)
    {
        chocado = true;
        Debug.Log(collider.name);
    }
    void OnTriggerExit(Collider other)
    {
        chocado = false;
    }
}
