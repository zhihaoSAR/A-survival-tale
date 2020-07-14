using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nivel1 : Nivel
{
    public string sio;
    public override NivelConfig generarConfig(string nivel)
    {
        return new NivelConfig();
    }
}
