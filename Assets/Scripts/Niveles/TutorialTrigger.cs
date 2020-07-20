using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public Tutorial tutorial;
    public int id;
    void OnTriggerEnter(Collider collider)
    {
        tutorial.mostrarTutorial(id);
        gameObject.SetActive(false);
    }

}
