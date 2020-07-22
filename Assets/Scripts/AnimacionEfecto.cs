using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionEfecto : MonoBehaviour
{
    public Material agua;
    Vector4 aguaVel = new Vector4(-3, 2, 1, 3);
    public void ActualizaAnimacion(int activa)
    {
        if(activa == 0)
        {
            agua.SetVector("_GSpeed", aguaVel);
        }
        else
        {
            agua.SetVector("_GSpeed", Vector4.zero);
        }
    }
}
