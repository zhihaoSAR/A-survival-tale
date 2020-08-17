using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_MoverCaja : TutorialTrigger
{
    void Update()
    {
        if(Controlador.control.player.estado == Player.Estado.PARADOCONCAJA)
        {
            tutorial.mostrarTutorial(id);
            gameObject.SetActive(false);
        }
    }
}
