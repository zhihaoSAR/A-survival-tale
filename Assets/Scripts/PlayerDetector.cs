using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Interactuable objeto;

    void OnTriggerStay(Collider other)
    {
        if ((transform.position - other.transform.position).sqrMagnitude
            < objeto.player.sqrDisObjeto)
        {
            objeto.player.objeto = objeto;
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if(objeto.player.objeto.Equals(objeto))
        {
            objeto.player.objeto = null;
        }
    }
}
