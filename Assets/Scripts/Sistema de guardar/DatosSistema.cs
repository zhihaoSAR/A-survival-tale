using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DatosSistema 
{
    public Dictionary<string, KeyCode> keys;
    public int hash;
    public int tipoControl;//0: nada 1:raton 2: wasd 3: 2boton
    public float inputTime;//tiempo de registro entre input
    public int tamanyoFuente;//0:pequeño 1:normal 2: grande
    public int tipoFuente;//0: TCM 1:TCB
    public int tipoCursor; //0: normal 1:especial
    public int tamanyoCursor; //0: pequeño 1:mediano 2:grande
    public int pantallaCompleta;//0:si 1:no
    public DatosSistema()
    {
        keys = new Dictionary<string, KeyCode>();
        keys.Add("arriba", KeyCode.W);
        keys.Add("abajo", KeyCode.S);
        keys.Add("izquierda", KeyCode.A);
        keys.Add("derecha", KeyCode.D);
        keys.Add("confirmar", KeyCode.Return);
        keys.Add("cancelar", KeyCode.Escape);
        keys.Add("A", KeyCode.Z);
        keys.Add("B", KeyCode.X);
        tipoControl = 0;
        inputTime = 0;
        tipoFuente = 0;
        tipoCursor = 0;
        tamanyoCursor = 0;
        tamanyoFuente = 1;
        pantallaCompleta = 1;
    }
    public bool comprobarDatos()
    {
        return hash == GetHashCode();
    }
    public override int GetHashCode()
    {
        int hashCode = 0;
        foreach (KeyValuePair<string, KeyCode> entry in keys)
        {
            hashCode +=(int) entry.Value;
        }
        hashCode += (int)inputTime;
        hashCode += tipoControl;
        hashCode += tamanyoFuente;
        hashCode += tipoFuente;
        hashCode += tipoCursor;
        hashCode += tamanyoCursor;
        hashCode += pantallaCompleta;
        return hashCode;
    }
}
