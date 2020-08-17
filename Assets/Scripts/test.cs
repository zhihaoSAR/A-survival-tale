using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
public class test : MonoBehaviour
{
    public PostProcessProfile profile;
    int mode = 0;
    public Text ratonpos, tamanyopos;
    void Start()
    {
        Debug.Log("dsafa");
    }
    public void mostrar(Vector2 raton,Vector2 tamanyo )
    {
        ratonpos.text = raton.ToString();
        tamanyopos.text = tamanyo.ToString();
    }
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
