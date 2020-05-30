using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Menu : MonoBehaviour
{
    
    public Controlador control;
    public DatosSistema datos;

    public abstract bool getActualNavegable(out GameObject[] nav);
    public abstract void cancelar();
    public abstract void abrirMenu(Controlador c,int op);




}
