using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggerComponente : MonoBehaviour
{
    public AreaTrigger areaTrigger;
    [HideInInspector]
    public GameObject objeto;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.isTrigger)
        {
            return;
        }
        objeto = collider.gameObject;
        areaTrigger.Activar();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        objeto = null;
        areaTrigger.Desactivar();
    }
}
