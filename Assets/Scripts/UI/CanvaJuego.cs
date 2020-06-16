using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvaJuego : Menu
{
    public RectTransform P_elegirDirCaja;
    public float tiempoTransicion = 0.2f;
    Coroutine elegirDir;
    [HideInInspector]
    public bool recordando = false;
    public RectTransform recordarInteractuar;
    Coroutine recordarInt;


    void Start()
    {
        control = Controlador.control;
        int tipoControl = control.datosSistema.tipoControl;
        string path = "Indicaciones/";
        if (tipoControl == 0)
        {
            path += "Icono Enter";
        }
        else if (tipoControl == 1)
        {
            path += "Icono Raton";
        }
        else if (tipoControl == 2)
        {
            path += "Icono Boton";
        }
        Debug.Log(path);
        Sprite imagenRecordar = Resources.Load<Sprite>(path);
        recordarInteractuar.GetComponent<Image>().sprite = imagenRecordar;
    }
    public override void abrirMenu(Controlador c, int op)
    {
        throw new NotImplementedException();

    }

    public override void cancelar()
    {
        throw new System.NotImplementedException();
    }

    public override void cerrarMenu()
    {
        throw new System.NotImplementedException();
    }

    public override bool getActualNavegable(out GameObject[] nav)
    {
        throw new System.NotImplementedException();
    }

    public void mostrarDireccion(Vector3 posicionPantalla,Transform caja,Quaternion rotacionCamara,Vector3[] posiciones,Action<Vector3> prepararInt)
    {
        P_elegirDirCaja.position = posicionPantalla;
        P_elegirDirCaja.eulerAngles = Vector3.zero;
        Vector3 dst = caja.forward;
        dst.y = dst.z;
        dst.z = 0;
        P_elegirDirCaja.transform.rotation *= Quaternion.FromToRotation( Vector3.up,dst); ;
        P_elegirDirCaja.transform.rotation *= rotacionCamara;
        for (int i = 0; i< P_elegirDirCaja.childCount; i++)
        {
            Button dir = P_elegirDirCaja.GetChild(i).GetComponent<Button>();
            dir.onClick.RemoveAllListeners();
            Vector3 pos = posiciones[i];
            dir.onClick.AddListener(() => { prepararInt(pos); });
        }
        elegirDir = StartCoroutine(popUp(P_elegirDirCaja));
    }
    public void cerrarDireccion()
    {
        if (elegirDir != null)
            StopCoroutine(elegirDir);
        P_elegirDirCaja.gameObject.SetActive(false);
    }

   IEnumerator popUp(RectTransform rect)
    {
        rect.localScale = Vector3.zero;
        rect.gameObject.SetActive(true);
        float now = 0;
        yield return null;
        while (now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            rect.localScale = Vector3.one * (now / tiempoTransicion);
            yield return null;
        }
        rect.localScale = Vector3.one;
    }
    public void MostrarControlInteractuar(bool activar)
    {
        if(activar)
        {
            recordarInt = StartCoroutine(popUp(recordarInteractuar));
            recordando = true;
        }
        else
        {
            if(recordarInt != null)
            {
                StopCoroutine(recordarInt);
                recordarInteractuar.gameObject.SetActive(false);
                recordando = false;
            }
        }
    }
   
}
