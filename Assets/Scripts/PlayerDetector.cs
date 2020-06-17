using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Interactuable objeto;

    void OnTriggerStay(Collider other)
    {
        objeto.player.asigObjeto(objeto);
        
        
        
    }
    void OnTriggerExit(Collider other)
    {
        objeto.player.desasigObjeto(objeto);
        
    }
}
