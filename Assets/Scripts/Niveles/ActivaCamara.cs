using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ActivaCamara : MonoBehaviour
{
    public CinemachineVirtualCamera camara;
    void OnTriggerEnter(Collider collider)
    {
        camara.gameObject.SetActive(true);
    }

}
