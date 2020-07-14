using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaControlador : MonoBehaviour
{
    public Nivel nivelActual;
    public int numNivelActual;
    public bool[] escenaActiva;

    void Start()
    {
        escenaActiva = new bool[3];
        for(int i = 0; i< escenaActiva.Length;i++)
        {
            escenaActiva[i] = false;
        }
    }

    public AsyncOperation cargarEscena(int id)
    {
        escenaActiva[id] = true;
        return SceneManager.LoadSceneAsync(escena2BuildId(id), LoadSceneMode.Additive);
    }
    public void descargarEscena(int id)
    {
        if(escenaActiva[id])
        {
            SceneManager.UnloadSceneAsync(escena2BuildId(id));
        }
    }
    public void cargarNivel(Nivel nivel)
    {
        nivelActual = nivel; 
    }
    public void cargarNivel(Nivel nivel,int num)
    {
        nivelActual = nivel;
        numNivelActual = num;
    }
    public int escena2BuildId(int nivel)
    {
        return nivel;
    }
}
