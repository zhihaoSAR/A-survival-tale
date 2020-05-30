using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RemapearTeclado : Pagina
{
    KeyCode? keyObtenido;
    Dictionary<KeyCode, string> keysInversa;
    Dictionary<string, TextMeshProUGUI> botonTextos;
    bool registrando=false;
    string botonActual;
    //---------------elementos para inicializar----------
    public TextMeshProUGUI[] Text_textos;
    public RectTransform[] Rect_Opciones;
    public TextMeshProUGUI Text_explicacion;
    public Button[] B_botonesRemap;

    //------------------publico-------------------
    //-------------------privada----------
    int tipoFuente = 0;
    int tamanyoFuente = 0;

    public override void inicializar(Controlador c, Configuracion m)
    {
        base.inicializar(c, m);
        DatosSistema datos = control.datosSistema;
        cambiarFuente(datos.tipoFuente);
        cambiarTamanyo(datos.tamanyoFuente);
        keysInversa = new Dictionary<KeyCode, string>();
        foreach (KeyValuePair<string, KeyCode> entry in datos.keys)
        {
            keysInversa.Add(entry.Value, entry.Key);
        }
        botonTextos = new Dictionary<string, TextMeshProUGUI>();
        for (int i = 0; i < B_botonesRemap.Length; i++)
        {
            Button boton = B_botonesRemap[i];
            TextMeshProUGUI text = boton.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            botonTextos.Add(boton.name, text);
            boton.onClick.AddListener(() => { remapearBoton(boton.name); });
            UpdateText(botonTextos[boton.name], Controlador.keys[boton.name].ToString());
        }
    }
    public void cambiarTamanyo(int tamanyo)
    {
        if (tamanyoFuente == tamanyo)
            return;
        Vector2 proporcion;
        switch (tamanyo)
        {
            case 0:
                proporcion = Vector2.one;
                Text_explicacion.fontSize = 30;
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                break;
            case 1:
                proporcion = new Vector2(1.25f, 1.25f);
                Text_explicacion.fontSize = 37;
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                break;
            case 2:
                proporcion = new Vector2(1.5f, 1.5f);
                Text_explicacion.fontSize = 45;
                foreach (RectTransform transform in Rect_Opciones)
                {
                    transform.localScale = proporcion;
                }
                break;
        }
        tamanyoFuente = tamanyo;
        control.datosSistema.tamanyoFuente = tamanyo;
    }
    //0:tcm 1:tcb
    public void cambiarFuente(int tipo)
    {
        if (tipoFuente == tipo)
            return;
        TMP_FontAsset fuente = control.getFont(tipo);
        Text_explicacion.font = fuente;
        foreach (TextMeshProUGUI text in Text_textos)
        {
            text.font = fuente;
        }
        tipoFuente = tipo;
        control.datosSistema.tipoFuente = tipo;

    }

    void remapearBoton(string boton)
    {
        if(!registrando)
        {
            control.uiCanControl = false;
            control.canNavegar = false;
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
                control.Mappear();
                StartCoroutine(FinalizarCaptura());
            }
        }
    }
    IEnumerator FinalizarCaptura()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        control.uiCanControl = true;
        control.canNavegar = true;
    }

    void UpdateText(TextMeshProUGUI boton, string keyName)
    {
        boton.text = keyName;
    }



}
