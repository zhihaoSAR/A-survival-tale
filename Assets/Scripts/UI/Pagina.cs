﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pagina : MonoBehaviour
{
    public GameObject[] navegable;
    [HideInInspector]
    public Controlador control;
    [HideInInspector]
    public Configuracion menu;
    public GameObject primeroElegido;

    public virtual void inicializar(Configuracion m)
    {
        control = Controlador.control;
        menu = m;
    }
}
