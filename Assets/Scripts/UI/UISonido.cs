using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISonido:MonoBehaviour
{

    public AudioClip jugar,
        cambioVMenuPrincipal,
        cambioVGeneral,
        cambioH,
        confirmarToggle,
        cambioPagina,
        configuracionfondo,
        salirBloque;
    [HideInInspector]
    public AudioClip cambioV;
    public AudioSource source;


    public void Play_cambioV()
    {
        source.PlayOneShot(cambioV);
    }
    public void Play_cambioH()
    {
        source.PlayOneShot(cambioH);
    }
    //id 0: fondo del Configuracion
    public void Play_BGM(int id)
    {
        source.Stop();
        source.clip = configuracionfondo;
        source.Play();
        source.loop = true;
    }
    public void Stop_BGM()
    {
        source.Stop();
    }
    public void Play_cambioPagina()
    {
        source.PlayOneShot(cambioPagina);
    }
    public void Play_salirBloque()
    {
        source.PlayOneShot(salirBloque);
    }
    public void Play_confirmarToggle()
    {
        source.PlayOneShot(confirmarToggle);
    }



}
