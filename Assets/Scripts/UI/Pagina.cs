using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pagina : MonoBehaviour
{
    public GameObject[] navegable;

    public abstract void inicializar(Controlador c);
}
