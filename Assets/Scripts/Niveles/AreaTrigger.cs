using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaTrigger : MonoBehaviour
{
    public AreaTriggerComponente[] triggers;
    public int contador;
    public bool unaVez = false;
    void Start()
    {
        contador = triggers.Length;
    }
    public abstract void Activar();
    public abstract void Desactivar();
}
