using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public RectTransform entrante;
    public RectTransform saliente;
    public Texture2D cursorped;
    public Texture2D cursormed;
    public Texture2D cursorgran;
    public Controlador c;
    public Pagina p;
    public Configuracion conf;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // c.uicontrol.initNavegacion(c.uicontrol.paginaActual);
            Debug.Log("start");
            StartCoroutine(prueba());

        }
    }
    IEnumerator prueba()
    {
        int x = 0;
        Debug.Log(x);
        while(x < 5)
        {
            Debug.Log(++x);
            yield return null;
        }
    }
}
