using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletarNivel : MonoBehaviour
{
    public Nivel nivel;
    void OnTriggerEnter(Collider collider)
    {
        nivel.completadoNivel();
        gameObject.SetActive(false);
    }
}
