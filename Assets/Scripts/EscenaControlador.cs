
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
    public void asignarNivel(Nivel nivel,int id)
    {
        if (nivelActual != null)
        {
            Controlador.control.guardarDatoJuego();
        }
        numNivelActual = id;
        nivelActual = nivel;
    }
    public void cargarEscenaIntermedio()
    {
        SceneManager.LoadScene(ESCENA_INTERMEDIO,LoadSceneMode.Additive);
        
    }
    public void iniciarJuego(DatosJuego datos)
    {
        numNivelActual = datos.nivelActual;
        List<AsyncOperation> lista = new List<AsyncOperation>();
        AsyncOperation op;
        finalizado = false;
        Controlador.control.iniciarPantallaCargar();
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
        Controlador control = Controlador.control;
        control.uiControlable = false;
        control.inputModule.desactivarRatonRegistrar = true;
        control.canvaActual = control.player.HUD;
        control.cargando = false;
        control.navegable = false;
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
