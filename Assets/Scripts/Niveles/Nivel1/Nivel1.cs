using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Nivel1 : Nivel
{
    //-------puzzle1--------------
    public Caja caja1;
    public GameObject cajaModelo1;
    public NavMeshObstacle obstaculo1;
    public AreaTrigger areaTrigger1;
    //--------puzzle2--------------



    public override NivelConfig generarConfig(string nivel)
    {
        NivelConfig nivel1 = new NivelConfig();
        //------------vector3--------------
        Vector3[] vector3 = new Vector3[2];
        vector3[0] = caja1.transform.position;
        vector3[1] = cajaModelo1.transform.position;
        nivel1.vector3 = vector3;
        //----------booleanos----------------
        bool[] booleanos = new bool[3];
        booleanos[0] = caja1.enabled;
        booleanos[1] = obstaculo1.enabled;
        booleanos[2] = areaTrigger1.gameObject.activeInHierarchy;
        nivel1.booleanos = booleanos;
        return nivel1;


    }
}
