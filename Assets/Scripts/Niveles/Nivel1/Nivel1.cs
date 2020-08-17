using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Nivel1 : Nivel
{
    public NivelConfigObj configuracionIni;
    //--------tutorial-------------
    public TutorialTrigger[] tutoriales;
    //-------caja que puede caer--------------
    public Caja[] cajaDesactivables;
    public GameObject[] cajaModelos;
    //-----------obstaculos---------------
    public NavMeshObstacle[] obstaculos;
    public AreaTrigger[] areaTriggers;
    //--------caja normal--------------
    public Caja[] cajas;
    //---------piedra----------
    public PiedraPuerta[] piedras;
    //--------completarNivel--------
    public CompletarNivel completar;
    //--------posicion inicio del jugador--------
    public GameObject posJugador;


    public override NivelConfig generarConfig()
    {
        NivelConfig nivel1 = new NivelConfig();

        //--------------------------------
        //-          BOOL                -
        //--------------------------------
        bool[] booleanos = new bool[15];
        int ind = 0;
        //--------tutorial----------------
        for (int i = 0;i<tutoriales.Length;i++,ind++)
        {
            booleanos[ind] = tutoriales[i].gameObject.activeInHierarchy;
        }
        //------------caja que puede caer--------------
        for (int i = 0; i < cajaDesactivables.Length; i++, ind++)
        {
            booleanos[ind] = cajaDesactivables[i].enabled;
        }
        //-----------obstaculos---------------
        for (int i = 0; i < obstaculos.Length; i++, ind++)
        {
            booleanos[ind] = obstaculos[i].enabled;
        }
        for (int i = 0; i < areaTriggers.Length; i++, ind++)
        {
            booleanos[ind] = areaTriggers[i].gameObject.activeInHierarchy;
        }
        //--------------piedra-----------------
        for (int i = 0; i < piedras.Length; i++, ind++)
        {
            booleanos[ind] = piedras[i].abierto;
        }
        booleanos[ind] = completado;
        //--------------------------------
        //-          INTEGER             -
        //--------------------------------
        int[] integers = new int[3];
        ind = 0;
        //-----------obstaculos--------------
        for (int i = 0; i < areaTriggers.Length; i++, ind++)
        {
            integers[ind] = areaTriggers[i].contador ;
        }

        //--------------------------------
        //-          VECTOR3             -
        //--------------------------------
        Vector3[] vector3 = new Vector3[11];
        ind = 0;
        //------------caja que puede caer--------------
        for (int i = 0; i < cajaDesactivables.Length; i++,ind++)
        {
            vector3[i] = cajaDesactivables[i].transform.position;
        }
        for (int i = 0; i < cajaModelos.Length; i++, ind++)
        {
            vector3[ind] = cajaModelos[i].transform.position;
        }
        //------------caja normal-----------
        for (int i = 0; i < cajas.Length; i++, ind++)
        {
            vector3[ind] = cajas[i].transform.position;
        }
        //--------------piedra-----------------
        for (int i = 0; i < piedras.Length; i++, ind++)
        {
            vector3[ind] = piedras[i].transform.position;
        }
        vector3[ind] = posJugador.transform.position;

        //--------------------------------
        //-          QUATERNION          -
        //--------------------------------
        Quaternion[] rotaciones = new Quaternion[2];
        ind = 0;
        //--------------piedra-----------------
        for (int i = 0; i < piedras.Length; i++, ind++)
        {
            rotaciones[ind] = piedras[i].transform.rotation;
        }

        nivel1.valores = integers;
        nivel1.vector3 = vector3;
        nivel1.booleanos = booleanos;
        nivel1.rotaciones = rotaciones;
        return nivel1;
    }
    public override void cargarConf(NivelConfig config)
    {
        //--------------------------------
        //-          BOOL                -
        //--------------------------------
        int ind = 0;
        //--------tutorial----------------
        for (int i = 0; i < tutoriales.Length; i++,ind++)
        {
            tutoriales[i].gameObject.SetActive(config.booleanos[ind]);
        }
        //------------caja que puede caer--------------
        for (int i = 0; i < cajaDesactivables.Length; i++, ind++)
        {
            if(config.booleanos[ind])
            {
                cajaDesactivables[i].activarCaja();
                
            }
            else
            {
                cajaDesactivables[i].desactivarCaja();
            }
        }
        if(config.booleanos[ind])
        {
            completado = false;
            completar.gameObject.SetActive(false);
        }
        //-----------obstaculos---------------
        for (int i = 0; i < obstaculos.Length; i++, ind++)
        {
            obstaculos[i].enabled = config.booleanos[ind];
        }
        for (int i = 0; i < areaTriggers.Length; i++, ind++)
        {
            areaTriggers[i].gameObject.SetActive(config.booleanos[ind]);

        }
        //--------------piedra-----------------
        for (int i = 0; i < piedras.Length; i++, ind++)
        {
            piedras[i].configEstado(config.booleanos[ind]);
        }

        //--------------------------------
        //-          INTEGER             -
        //--------------------------------
        ind = 0;
        //-----------obstaculos--------------
        for (int i = 0; i < areaTriggers.Length; i++, ind++)
        {
            areaTriggers[i].contador = config.valores[ind];
        }

        //--------------------------------
        //-          VECTOR3             -
        //--------------------------------
        ind = 0;
        //------------caja que puede caer--------------
        for (int i = 0; i < cajaDesactivables.Length; i++, ind++)
        {
            cajaDesactivables[i].transform.position = config.vector3[ind];
        }
        for (int i = 0; i < cajaModelos.Length; i++, ind++)
        {
            cajaModelos[i].transform.position = config.vector3[ind];
        }
        //------------caja normal---------------
        for (int i = 0; i < cajas.Length; i++, ind++)
        {
            cajas[i].transform.position = config.vector3[ind]; ;
        }
        //--------------piedra-----------------
        for (int i = 0; i < piedras.Length; i++, ind++)
        {
            piedras[i].transform.position = config.vector3[ind];
        }
        //--------------------------------
        //-          QUATERNION          -
        //--------------------------------
        ind = 0;
        //--------------piedra-----------------
        for (int i = 0; i < piedras.Length; i++, ind++)
        {
            piedras[i].transform.rotation = config.rotaciones[ind];
        }
    }

    public override void reintentarNivel()
    {
        NivelConfig config = new NivelConfig();
        NivelConfigObj.Obj2Config(configuracionIni, ref config);
        Controlador.control.player.ReiniciarJugador();
        Controlador.control.player.moverJugador(config.vector3[config.vector3.Length - 1]);
        cargarConf(config);
        
    }
    public override void completadoNivel()
    {
        base.completadoNivel();
        Controlador.control.datosJuego.mapaisala = "Mapa/Mapa_selva";
        Controlador.control.player.HUD.mapa.actualizarMapa();
    }
}
