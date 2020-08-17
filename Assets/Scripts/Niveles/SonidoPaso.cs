using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoPaso : MonoBehaviour
{
    public AudioClip sonidoPaso;
    void OnTriggerEnter(Collider collider)
    {
        Controlador.control.player.cambiarSonidoPaso(sonidoPaso);
    }
}
