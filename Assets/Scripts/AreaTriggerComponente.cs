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
        objeto = collider.gameObject;
        areaTrigger.Activar();
    }
    void OnTriggerExit(Collider other)
    {
        objeto = null;
        areaTrigger.Desactivar();
    }
}
