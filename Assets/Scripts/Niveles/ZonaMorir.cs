using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaMorir : MonoBehaviour
{
    public string anim;
    void OnTriggerEnter(Collider collider)
    {
        Controlador.control.morir(anim);
    }
}
