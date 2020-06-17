using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DatosSistema 
{
    public Dictionary<string, KeyCode> keys;
    public int hash;
    
    //------------------visualizacion-----------
    public int tamanyoFuente;//0:30 1:1.25 2: 1.5
    public int tipoFuente;//0: TCM 1:opendyslexic
    public int tipoCursor; //0: normal 1:especial
    public int tamanyoCursor; //0: normal 1:1.25 2:1.5
    public int pantallaCompleta;//0:no 1:si
    //-------------animacion---------------------------
    public int activarNpcAnim;//0:no 1:si
    public int activarPeligroAnim;//0:no 1:si
    public int activarDecoracionAnim;//0:no 1:si
    //-----------sonido y volumen---------------------
    public int audioModo;//0:estéreo 1:mono
    public float ambienteVol,peligroVol,interaccionVol,interfazVol,pasoVol;//0,25,50,75,100
    //-----------------principal---------------------------
    public int invencibilidad;//0:desactivado 1: activado
    //---------------control---------------------
    public int tipoControl;//0:wasd 1: raton 2: 2boton
    public float inputTime;//tiempo de registro entre input 0s,0.5s,1s
    public int velocidad; // 0:lenta 1:media 2:rapida
    //------------------contraste--------------
    public Color color_fondo,color_personaje,color_interactivo;
    //-------------------globales-----------------
    public bool finalizadoConf;

    public DatosSistema()
    {
        keys = new Dictionary<string, KeyCode>();
        keys.Add("arriba", KeyCode.W);
        keys.Add("abajo", KeyCode.S);
        keys.Add("izquierda", KeyCode.A);
        keys.Add("derecha", KeyCode.D);
        keys.Add("confirmar", KeyCode.Return);
        keys.Add("cancelar", KeyCode.Escape);
        keys.Add("A", KeyCode.J);
        keys.Add("B", KeyCode.K);
        keys.Add("deshacerWASD", KeyCode.Z);
        keys.Add("deshacer2B", KeyCode.L);
        //------------------visualizacion-----------
        tamanyoFuente = 0;
        tipoFuente = 0;
        tipoCursor = 0;
        tamanyoCursor = 0;
        pantallaCompleta = 0;
        //-------------animacion---------------------------
        activarNpcAnim = 1;
        activarPeligroAnim = 1;
        activarDecoracionAnim = 1;
        //-----------sonido y volumen---------------------
        audioModo = 0;
        ambienteVol = 100;
        peligroVol = 100;
        interaccionVol = 100;
        interfazVol = 100;
        pasoVol = 100;
        //-----------------principal---------------------------
        invencibilidad = 0;
        //---------------control---------------------
        tipoControl = 1;
        inputTime = 0.5f;
        velocidad = 1;
        //------------------contraste--------------
        color_fondo = Color.clear;
        color_personaje = Color.clear;
        color_interactivo = Color.clear;
        //-------------------globales-----------------
        finalizadoConf = false;
        
        

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
        hashCode += activarNpcAnim;
        hashCode += activarPeligroAnim;
        hashCode += activarDecoracionAnim;
        hashCode += audioModo;
        hashCode += (int)(ambienteVol + peligroVol + interaccionVol + interfazVol + pasoVol);
        hashCode += invencibilidad;
        hashCode += (int)(color_fondo.grayscale * 256 + 
                            color_personaje.grayscale * 256 + 
                            color_interactivo.grayscale * 256);

        return hashCode;
    }
}
