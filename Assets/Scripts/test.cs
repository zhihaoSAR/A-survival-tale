using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform testObj,other;
    public Controlador c;
    public Pagina p;
    public Material mymaterial;
    public Configuracion conf;
    Coroutine a = null;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            mymaterial.color = Color.blue;
            Destroy(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            mymaterial.color = Color.yellow;
            Destroy(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
