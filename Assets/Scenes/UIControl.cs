﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIControl : MonoBehaviour
{
    public Pagina canvaActual;
    public bool dosBotonModo = true;
    [HideInInspector]
    public bool pausaNav = false;
    Stack<int> navIndGuardado;
    Stack<GameObject[]> navGuardado;
    GameObject[] navegacion;
    int navInd;
    public float navTime = 1f;
    MyInputModule inputModulo;
    EventSystem eventSystem;
    public Sprite cuadroMarco;
    Cuadro actual;
    Coroutine navCorotina;

    void Start()
    {
        navGuardado = new Stack<GameObject[]>();
        navIndGuardado = new Stack<int>();
        inputModulo = GetComponent<MyInputModule>();
        eventSystem = GetComponent<EventSystem>();
        if(dosBotonModo)
        {
            initNavegacion(canvaActual);
        }
    }
    public void initNavegacion(Pagina p)
    {
        navegacion = p.navegable;
        navInd = 0;
        actual = null;
        elegirObjeto(navegacion[0]);
        navCorotina = StartCoroutine("Navegacion");
    }
    IEnumerator Navegacion()
    {
        while(dosBotonModo)
        {
            yield return new WaitForSecondsRealtime(navTime);
            if(!pausaNav)
            {
                if (++navInd == navegacion.Length)
                {
                    navInd = 0;
                }
                elegirObjeto(navegacion[navInd]);
            }
        }
    }
    void elegirObjeto(GameObject o)
    {
        Cuadro cuadro;
        if(o.TryGetComponent<Cuadro>(out cuadro))
        {
            cuadro.image.enabled = true;
            
        }
        if (actual != null)
        {
            actual.image.enabled = false;
        }
        actual = cuadro;
        eventSystem.SetSelectedGameObject(o);
    }

    public void confirmar()
    {
        Debug.Log("11111111");
        if (actual != null)
        {
            apilarNavegacion(actual.navegable);
        }
    }
    public void cancelar()
    {
        if(navGuardado.Count >0)
        {
            navegacion = navGuardado.Pop();
            navInd = navIndGuardado.Pop();
            elegirObjeto(navegacion[navInd]);
        }
    }
    public void apilarNavegacion(GameObject[] navegable)
    {
        Debug.Log("2222222");
        navGuardado.Push(navegacion);
        navIndGuardado.Push(navInd);
        navegacion = navegable;
        navInd = 0;
        StopCoroutine(navCorotina);
        navCorotina = StartCoroutine("Navegacion");
        elegirObjeto(navegacion[navInd]);

    }
}
