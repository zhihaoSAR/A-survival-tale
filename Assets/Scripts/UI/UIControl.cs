using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIControl : MonoBehaviour
{
    public Pagina paginaActual;
    public bool dosBotonModo = true;
    [HideInInspector]
    public bool pausaNav = false;
    Stack<int> navIndGuardado;
    Stack<GameObject[]> navGuardado;
    Stack<Cuadro> cuadrosGuardado;
    GameObject[] navegacion;
    int navInd;
    public float navTime = 1f;
    public float tiempoActual = 0;
    MyInputModule inputModulo;
    EventSystem eventSystem;
    Cuadro actual;
    Coroutine navCorotina;
    public Controlador control;

    void Start()
    {
        cuadrosGuardado = new Stack<Cuadro>();
        navGuardado = new Stack<GameObject[]>();
        navIndGuardado = new Stack<int>();
        inputModulo = GetComponent<MyInputModule>();
        eventSystem = GetComponent<EventSystem>();
        if(dosBotonModo && paginaActual != null)
        {
            //initNavegacion(paginaActual);
        }
    }
    public void iniNavegacion(GameObject[] navegable)
    {
        LimpiarNav();
        navegacion = navegable;
        navInd = 0;
        tiempoActual = 0;
        if(actual!= null)
        {
            actual.image.enabled = false;
        }
        actual = null;
        eventSystem.SetSelectedGameObject(navegacion[0]);
        if (navCorotina != null)
            StopCoroutine(navCorotina);
        navCorotina = StartCoroutine("Navegacion");
    }
    public void LimpiarNav()
    {
        cuadrosGuardado.Clear();
        navIndGuardado.Clear();
        navGuardado.Clear();
    }
    IEnumerator Navegacion()
    {
        while(dosBotonModo)
        {
            if(tiempoActual < navTime)
            {
                tiempoActual += Time.unscaledDeltaTime;
            }
            else if(!pausaNav)
            {
                if (++navInd == navegacion.Length)
                {
                    navInd = 0;
                }
                eventSystem.SetSelectedGameObject(navegacion[navInd]);
                tiempoActual = 0;
            }
            yield return null;
        }
    }
    /*
    void elegirObjeto(GameObject o)
    {
        Cuadro cuadro;
        if (actual != null)
        {
            actual.image.enabled = false;
        }
        if (o.TryGetComponent<Cuadro>(out cuadro))
        {
            cuadro.image.enabled = true;
        }
        actual = cuadro;
        eventSystem.SetSelectedGameObject(o);
    }
    */
    public void confirmar()
    {
        if (!dosBotonModo)
            return;
        Cuadro cuadro;
        if ( navegacion[navInd].TryGetComponent<Cuadro>(out cuadro))
        {
            cuadro.image.enabled = true;
            if(actual != null)
            {
                actual.image.enabled = false;
                cuadrosGuardado.Push(actual);
            }
            actual = cuadro;
            apilarNavegacion(actual.navegable);
        }
    }
    public bool cancelar()
    {
        if (!dosBotonModo || !control.uiCanControl)
            return false;
        bool quedaNav = false;
        if (navGuardado.Count >0)
        {
            navegacion = navGuardado.Pop();
            navInd = navIndGuardado.Pop();
            eventSystem.SetSelectedGameObject(navegacion[navInd]);
            quedaNav = true;
        }
        if(actual != null)
        {
            actual.image.enabled = false;
        }
        if(cuadrosGuardado.Count>0)
        {
            actual = cuadrosGuardado.Pop();
            actual.image.enabled = true;
        }
        else
        {
            actual = null;
        }
        return quedaNav;
    }
    public void apilarNavegacion(GameObject[] navegable)
    {
        navGuardado.Push(navegacion);
        navIndGuardado.Push(navInd);
        navegacion = navegable;
        navInd = 0;
        if(navCorotina!=null)
            StopCoroutine(navCorotina);
        navCorotina = StartCoroutine("Navegacion");
        eventSystem.SetSelectedGameObject(navegacion[navInd]);

    }
}
