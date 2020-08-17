using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_posTrigger : TutorialTrigger
{
    void OnTriggerEnter(Collider collider)
    {
        tutorial.mostrarTutorial(id);
        gameObject.SetActive(false);
    }
}
