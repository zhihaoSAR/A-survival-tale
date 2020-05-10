using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RemapearTeclado : MonoBehaviour
{
    Controlador controlador;
    public KeyCode key;
    KeyCode? keyObtenido;
    Dictionary<KeyCode, string> keysInversa;
    Dictionary<string, TextMeshProUGUI> botonTextos;
    bool registrando=false;
    MyInputModule inputModule;
    string botonActual;
    void Start()
    {
        controlador = GameObject.Find("Control").GetComponent<Controlador>();
        keysInversa = new Dictionary<KeyCode, string>();
        foreach (KeyValuePair<string, KeyCode> entry in Controlador.keys)
        {
            keysInversa.Add(entry.Value, entry.Key);
        }
        botonTextos = new Dictionary<string, TextMeshProUGUI>();
        for (int i = 0; i< transform.childCount;i++)
        {
            Transform boton = transform.GetChild(i);
            TextMeshProUGUI text = boton.GetChild(1).GetComponent<TextMeshProUGUI>() ;
            botonTextos.Add(boton.name, text);
            boton.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>{ remapearBoton(boton.name); });
            UpdateText(botonTextos[boton.name], Controlador.keys[boton.name].ToString());
        }
        inputModule = GameObject.Find("EventSystem").GetComponent<MyInputModule>();
    }

    void remapearBoton(string boton)
    {
        if(!registrando)
        {
            inputModule.canControl = false;
            inputModule.pausaNav = true;
            keyObtenido = null;
            UpdateText(botonTextos[boton], " ");
            botonActual = boton;
            StartCoroutine(InicializarCaptura());
            keysInversa.Remove(Controlador.keys[boton]);
        }
        
    }
    IEnumerator InicializarCaptura()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        registrando = true;
    }

    void OnGUI()
    {
        if(registrando)
        {
            if (Event.current.isKey )
            {
                keyObtenido =  Event.current.keyCode;
                registrando = false;
                string botonConflicto;
                if( keysInversa.TryGetValue((KeyCode)keyObtenido, out botonConflicto))
                {
                    keysInversa.Remove(Controlador.keys[botonConflicto]);
                    Controlador.keys[botonConflicto] = Controlador.keys[botonActual];
                    TextMeshProUGUI textoConflicto;
                    if(botonTextos.TryGetValue(botonConflicto,out textoConflicto))
                    {
                        UpdateText(textoConflicto, Controlador.keys[botonConflicto].ToString());
                    }
                    keysInversa.Add(Controlador.keys[botonConflicto], botonConflicto);
                    
                }
                Controlador.keys[botonActual] = (KeyCode)keyObtenido;
                keysInversa.Add((KeyCode)keyObtenido, botonActual);
                UpdateText(botonTextos[botonActual], keyObtenido.ToString());
                controlador.Mappear();
                StartCoroutine(FinalizarCaptura());
            }
        }
    }
    IEnumerator FinalizarCaptura()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        inputModule.canControl = true;
        inputModule.pausaNav = false;
    }

    void UpdateText(TextMeshProUGUI boton, string keyName)
    {
        boton.text = keyName;
    }



}
