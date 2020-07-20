using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonSuelo : MonoBehaviour
{
    public Puerta puerta;
    void OnTriggerEnter(Collider collider)
    {
        puerta.Abrir(true);
    }
    void OnTriggerExit(Collider other)
    {
        puerta.Abrir(false);
    }
}
