using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class NivelConfig 
{
    public Vector3[] vector3;
    public Quaternion[] rotaciones;
    public int[] valores;
    public bool[] booleanos;


    public override int GetHashCode()
    {
        int hashCode = 0;
        for(int i  = 0;i< valores.Length;i++)
        {
            hashCode += valores[i]<<i;
        }
        for (int i = 0; i < booleanos.Length; i++)
        {
            hashCode += Convert.ToInt32(booleanos[i]) <<i;
        }
        return hashCode;
    }
}
