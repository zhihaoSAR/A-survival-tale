﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configuracion : Menu
{
    //---------------menus-----------------
    public Pagina seleccionControl;
    public Pagina menuInicio;
    public Pagina configuracionInicion;


    //--------------constantes------------
    const float tiempoTransicion = 0.5f;

    //--------------variables------------------
    Pagina paginaActual;
    Stack<Pagina> paginasAnteriores;
    bool menuIni;

    public override void cancelar()
    {
        if (paginaActual.gameObject.Equals(seleccionControl.gameObject) || paginaActual.gameObject.Equals(menuInicio.gameObject))
            return;
        if (paginasAnteriores.Count == 0)
            StartCoroutine(TranscicionIniFinMenu(paginaActual.GetComponent<RectTransform>(), false));
        RectTransform pSal = paginaActual.GetComponent<RectTransform>();
        paginaActual = paginasAnteriores.Pop();
        RectTransform pEnt = paginaActual.GetComponent<RectTransform>();
        iniciarPagina();
        StartCoroutine(TranscicionCambiarPagina(pEnt, pSal, true));
        control.S_cambioPagina();
    }

    void Inicializacion(Controlador c)
    {
        control = c;
        datos = control.datosSistema;
        paginasAnteriores = new Stack<Pagina>();
    }
    public void limpiarPagAnt()
    {
        paginasAnteriores.Clear();
    }

    public void abrirPagina(Pagina p)
    {
        RectTransform pSal = paginaActual.GetComponent<RectTransform>();
        RectTransform pEnt = p.GetComponent<RectTransform>();
        paginasAnteriores.Push(paginaActual);
        paginaActual = p;
        if(paginaActual.Equals(menuInicio))
        {
            menuIni = true;
        }
        else
        {
            menuIni = false;
        }
        control.Set_cambioV(menuIni);
        iniciarPagina();
        StartCoroutine(TranscicionCambiarPagina(pEnt, pSal, false));
        control.S_cambioPagina();
        
    }
    public void iniciarPagina()
    {
        paginaActual.gameObject.SetActive(true);
        paginaActual.inicializar(control, this);
        control.eventSystem.firstSelectedGameObject = paginaActual.primeroElegido;
        if(datos.tipoControl == 0)
        {
            control.eventSystem.SetSelectedGameObject(paginaActual.primeroElegido);
        }
        else
        {
            control.eventSystem.SetSelectedGameObject(null);
        }
        
        if (datos.tipoControl == 2)
        {
            control.iniNavegacion(paginaActual.navegable);
        }
    }
    //op x:
    public override void abrirMenu(Controlador c, int op)
    {
        Inicializacion(c);
        menuIni = true;
        control.Set_cambioV(menuIni);
        paginaActual = menuInicio;
        iniciarPagina();
        control.S_musicaFondo(0);
        StartCoroutine(TranscicionIniFinMenu(menuInicio.GetComponent<RectTransform>(), true));
    }
    public override void cerrarMenu()
    {
        control.S_musicaFondo(0);
        StartCoroutine(TranscicionIniFinMenu(menuInicio.GetComponent<RectTransform>(), true));
    }


    public IEnumerator TranscicionCambiarPagina(RectTransform entrante, RectTransform saliente, bool izqAder)
    {
        int dir;
        if (izqAder)
            dir = -1;
        else
            dir = 1;
        Vector2 dst = new Vector2(saliente.rect.width, 0);
        bool antes_uicanControl = control.uiControlable;
        bool antes_canNavegar = control.canNavegar;
        control.uiControlable = false;
        control.canNavegar = false;
        entrante.anchoredPosition = saliente.anchoredPosition + dst * dir;
        float now = 0;
        Vector2 movimiento;
        Vector2 entranteOri = entrante.anchoredPosition,
                salienteOri = saliente.anchoredPosition;
        dir *= -1;
        yield return null;
        while (now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            movimiento = (now / tiempoTransicion) * dst * dir;
            entrante.anchoredPosition = entranteOri + movimiento;
            saliente.anchoredPosition = salienteOri + movimiento;
            yield return null;
        }
        movimiento = dst * dir;
        entrante.anchoredPosition = entranteOri + movimiento;
        saliente.anchoredPosition = salienteOri + movimiento;
        control.uiControlable = antes_uicanControl;
        control.canNavegar = antes_canNavegar;
        saliente.gameObject.SetActive(false);
    }
    public IEnumerator TranscicionIniFinMenu(RectTransform rect,bool ini)
    {
        int dir;
        Vector2 dst;
        if (ini)
        {
            dir = -1;
            dst = new Vector2(0, rect.rect.height);
            rect.anchoredPosition = new Vector2(0, -rect.rect.height);
        }
        else
        {
            dir = 1;
            dst = new Vector2(0, rect.rect.height);
            rect.anchoredPosition = Vector2.zero;
        }
        bool antes_uicanControl = control.uiControlable;
        bool antes_canNavegar = control.canNavegar;
        bool antes_canControl = control.controlable;
        control.uiControlable = false;
        control.canNavegar = false;
        control.controlable = false;
        float now = 0;
        Vector2 movimiento;
        Vector2 rectOri = rect.anchoredPosition;
        dir *= -1;
        yield return null;
        while (now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            movimiento = (now / tiempoTransicion) * dst * dir;
            rect.anchoredPosition = rectOri + movimiento;
            yield return null;
        }
        movimiento = dst * dir;
        rect.anchoredPosition = rectOri + movimiento;
        control.uiControlable = antes_uicanControl;
        control.canNavegar = antes_canNavegar;
        control.controlable = antes_canControl;
        if (!ini)
            rect.gameObject.SetActive(false);
    }



    public override bool getActualNavegable(out GameObject[] nav)
    {
        if (paginaActual != null)
        {
            nav = paginaActual.navegable;
            return true;
        }
        nav = null;
        return false;
    }
}