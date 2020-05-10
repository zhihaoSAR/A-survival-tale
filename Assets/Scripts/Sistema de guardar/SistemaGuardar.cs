using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;

public static class SistemaGuardar 
{
    public static bool guardarDatosSistema(DatosSistema datos)
    {
        string path = Application.dataPath + "/system.dat";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        Dictionary<string, string> dat = new Dictionary<string, string>();
        datos.hash = datos.GetHashCode();
        formatter.Serialize(stream,datos);
        
        stream.Close();
        return true;

    }
    public static bool cargarDatosSistema(out DatosSistema datos)
    {
        string path = Application.dataPath + "/system.dat";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            datos = formatter.Deserialize(stream) as DatosSistema;
            stream.Close();
            if (datos.comprobarDatos())
                return true;
        }
        datos = new DatosSistema();

        return false;
    }
}
