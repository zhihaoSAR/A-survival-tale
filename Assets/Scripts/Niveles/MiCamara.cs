using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class MiCamara : MonoBehaviour
{
    public bool follow = true, lookAt = true;

    void OnEnable()
    {
        CinemachineVirtualCamera camara = GetComponent<CinemachineVirtualCamera>();
        if(Controlador.control.camaraPrincipal != null)
        {
            Controlador.control.camaraPrincipal.gameObject.SetActive(false);
            
        }
        Controlador.control.camaraPrincipal = camara;
        if (follow)
            camara.Follow = Controlador.control.player.transform;
        if(lookAt)
            camara.LookAt = Controlador.control.player.transform;
    }

}
