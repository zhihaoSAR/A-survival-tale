using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    public Transform other;
    public Controlador c;
    public Pagina p;
    public Material mymaterial;
    public Configuracion conf;
    public NavMeshSurface surface;
    Coroutine a = null;
    public Animator anima;
    public GameObject testObj;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            Controlador.control.cerrarJuego();
        }



    }
    IEnumerator prueba()
    {
        int x = 0;
        Debug.Log("empiezo");
        while (x < 5)
        {
            x++;
            yield return null;
        }
        Debug.Log("acabado");
    }
}
