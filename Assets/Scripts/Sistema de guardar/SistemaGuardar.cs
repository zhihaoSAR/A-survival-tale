using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System;

public static class SistemaGuardar 
{
    public static bool guardarDatosSistema(DatosSistema datos)
    {
        string path = Application.dataPath + "/system.dat";
        BinaryFormatter formatter = generarFormatter();
        SurrogateSelector selector = new SurrogateSelector();

        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        datos.hash = datos.GetHashCode();
        formatter.Serialize(stream,datos);
        
        stream.Close();
        return true;

    }
    public static bool cargarDatosSistema(out DatosSistema datos)
    {
        string path = Application.dataPath + "/system.dat";
        try
        {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = generarFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                datos = formatter.Deserialize(stream) as DatosSistema;
                stream.Close();
                if (datos.comprobarDatos())
                    return true;
            }
        }
        catch(Exception)
        {
            goto defecto;
        }
        defecto:
        datos = new DatosSistema();
        return false;
    }
    public static bool guardarDatosJuego(DatosJuego datos)
    {
        string path = Application.dataPath + "/save.dat";
        BinaryFormatter formatter = generarFormatter();
        SurrogateSelector selector = new SurrogateSelector();

        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        datos.hash = datos.GetHashCode();
        formatter.Serialize(stream, datos);

        stream.Close();
        return true;

    }
    public static bool cargarDatosJuego(out DatosJuego datos)
    {
        string path = Application.dataPath + "/save.dat";
        try
        {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = generarFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                datos = formatter.Deserialize(stream) as DatosJuego;
                stream.Close();
                if (datos.comprobarDatos())
                    return true;
            }
        }
        catch (Exception)
        {
            goto defecto;
        }
    defecto:
        datos = new DatosJuego();
        return false;
    }
    public static bool existeDatoJuego()
    {
        return File.Exists(Application.dataPath + "/save.dat");
    }

    static BinaryFormatter generarFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All),
            new Vector3SerializationSurrogate());
        selector.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All),
            new ColorSerializationSurrogate());
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All),
            new QuaternionSerializationSurrogate());
        formatter.SurrogateSelector = selector;
        return formatter;
    }
}
