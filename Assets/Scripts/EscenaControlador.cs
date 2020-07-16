
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class EscenaControlador : MonoBehaviour
{
    [HideInInspector]
    public Nivel nivelActual;
    [HideInInspector]
    public int numNivelActual;
    [HideInInspector]
    public bool[] escenaActiva;
    [HideInInspector]
    public bool finalizado;

    const int ESCENA_INTERMEDIO = 2;
    void Start()
    {
        escenaActiva = new bool[3];
        for(int i = 0; i< escenaActiva.Length;i++)
        {
            escenaActiva[i] = false;
        }
    }
    public void cargarEscenaIntermedio()
    {
        SceneManager.LoadScene(ESCENA_INTERMEDIO);
    }
    public void iniciarJuego(DatosJuego datos)
    {
        List<AsyncOperation> lista = new List<AsyncOperation>();
        AsyncOperation op;
        finalizado = false;
        for(int i = 0; i< datos.escenaActiva.Length;i++)
        {
            if(datos.escenaActiva[i] != 0)
            {
                 op =cargarEscena(datos.escenaActiva[i]);
                if(op != null)
                {
                    lista.Add(op);
                }
            }
        }
        StartCoroutine(cargando(lista));
    }
    IEnumerator cargando(List<AsyncOperation> lista)
    {
        yield return new WaitForSecondsRealtime(2);
        while(!finalizado)
        {
            yield return null;
            foreach(AsyncOperation op in lista)
            {
                if(!op.isDone)
                {
                    continue;
                }
            }
            finalizado = true;
        }
    }

    public AsyncOperation cargarEscena(int id)
    {
        if (!escenaActiva[id])
        {
            escenaActiva[id] = true;
            return SceneManager.LoadSceneAsync(escena2BuildId(id), LoadSceneMode.Additive);
        }
        return null;
        
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
        return nivel+2;
    }
}
