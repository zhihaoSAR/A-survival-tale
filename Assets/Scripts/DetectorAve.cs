using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorAve : MonoBehaviour
{
    public Ave ave;
    void OnTriggerEnter(Collider other)
    {
        ave.chocado = true;
        ave.objetoChocado = other.gameObject;
        Debug.Log(other.gameObject.name);


    }

}
