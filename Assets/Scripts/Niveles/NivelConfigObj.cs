using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class NivelConfigObj : ScriptableObject
{

    public Vector3[] vector3;
    public Quaternion[] rotaciones;
    public int[] valores;
    public bool[] booleanos;

    public static void Obj2Config(NivelConfigObj obj, ref NivelConfig datos)
    {
        datos.vector3 = obj.vector3;
        datos.rotaciones = obj.rotaciones;
        datos.valores = obj.valores;
        datos.booleanos = obj.booleanos;
    }
    public static void config2Obj(ref NivelConfigObj obj, NivelConfig datos)
    {
        obj.vector3 = datos.vector3;
        obj.rotaciones = datos.rotaciones;
        obj.valores = datos.valores;
        obj.booleanos = datos.booleanos;
    }
}
