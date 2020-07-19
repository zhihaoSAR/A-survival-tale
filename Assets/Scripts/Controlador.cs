

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controlador : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys;
    public MyInputModule inputModule;
    public DatosSistema datosSistema;
    public DatosJuego datosJuego;
    public UIControl uicontrol;
    static float tiempoTransicion = 0.15f;
    public EventSystem eventSystem;
    public UISonido uisonido;
    public EscenaControlador escenaControlador;
    [HideInInspector]
    public Player player;
    public bool controlable = true;
    

    public Menu canvasActual;
    TMP_FontAsset tcm,openDyslexic;
    [SerializeField]
    public AudioMixer audioMixer;
    //----------elegir color--------------
    public RectTransform rect_color;
    public Texture2D tex_color;
    public Image marca;
    public RectTransform panelElegirColor;
    public Image preview;
    bool elegiendoColor =false;
    Action<Color> completado;
    Camera mainCamera;
    Vector2 tamanyoColor;
    bool persigueRaton = false;
    int velocidad = 100;
    public Image I_registrando;
    Action<Color> acabado;
    //----------popUp--------------------
    bool ultUiControlable, ultControlable, ultJugadorControlable;
    //--------------salir-------------
    bool cerrandoJuego = false;
    public RectTransform panelSalir;
    public TextMeshProUGUI[] salirTextos;
    public GameObject[] salirNav;
    bool cerrar = false;
    //-----------contraste controlador-------------
    public ContrasteControlador contrastePrefab;
    ContrasteControlador contrasteControlador;
    //------------pantalla de cargar-------------
    public RectTransform panelCargar;
    [HideInInspector]
    public bool cargando = false;
    //-------------Tutorial------------
    public RectTransform panelTutorial;
    public TextMeshProUGUI tutorial_texto;
    public Image tutorial_imagen;
    public Sprite imagenDefecto;

    public static Controlador control;
    


    
    void Start()
    {
        control = this;
        SistemaGuardar.cargarDatosSistema(out datosSistema);
        SistemaGuardar.cargarDatosJuego(out datosJuego);
        keys = datosSistema.keys;
        Mappear();
        Application.wantsToQuit += cerrarJuego;
#if ESCENA_PRUEBA
        uiControlable = false;
        inputModule.desactivarRatonRegistrar = true;

#else
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(uicontrol.gameObject);
        DontDestroyOnLoad(canvasActual.gameObject);
        
        canvasActual.abrirMenu(1);

#endif

    }
    public void NuevaPartida()
    {
        datosJuego = new DatosJuego();
        EmpezarJuego();
    }
    public void Continuar()
    {
         SistemaGuardar.cargarDatosJuego(out datosJuego);
        EmpezarJuego();
    }

    public void EmpezarJuego()
    {
        canvasActual.cerrarMenu(0);
        escenaControlador.cargarEscenaIntermedio();
        escenaControlador.iniciarJuego(datosJuego);
        StartCoroutine(cargarEscena());
    }

    public void mostrarTutorial(Sprite[] imagenes,string[] textos)
    {
        jugadorControlable = false;
        panelTutorial.gameObject.SetActive(true);
        tutorial_texto.text = textos[0];
        if(imagenes[0] != null)
        {
            tutorial_imagen.sprite = imagenes[0];
        }
        else
        {
            tutorial_imagen.sprite = imagenDefecto;
        }
        int tamanyoFuente;
        switch(datosSistema.tamanyoFuente)
        {
            case 1:tamanyoFuente = 37;
                break;
            case 2: tamanyoFuente = 45;
                break;
            default:
                tamanyoFuente = 30;
                break;
        }
        tutorial_texto.fontSize = tamanyoFuente;
        panelTutorial.SetParent(canvasActual.transform);
        StartCoroutine(mostrarTutorial_Coroutine(imagenes, textos));

    }
    IEnumerator mostrarTutorial_Coroutine(Sprite[] imagenes, string[] textos)
    {
        StartCoroutine(PopUp(panelTutorial));
        yield return new WaitForSecondsRealtime(1.5f);
        bool acabado = false;
        int ind = 1;
        while(!acabado)
        {
            yield return null;
            if(datosSistema.tipoControl == 0)
            {
                if(Input.GetKeyUp(keys["confirmar"]) || Input.GetKeyUp(keys["cancelar"]))
                {
                    goto siguiente;
                }
            }
            if (datosSistema.tipoControl == 1)
            {
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                {
                    goto siguiente;
                }
            }
            if (datosSistema.tipoControl == 2)
            {
                if (Input.GetKeyUp(keys["A"]) || Input.GetKeyUp(keys["B"]))
                {
                    goto siguiente;
                }
            }
            continue;
        siguiente:
            if(ind < textos.Length)
            {
                tutorial_texto.text = textos[ind];
                if(imagenes[ind] != null)
                {
                    tutorial_imagen.sprite = imagenes[ind];
                }
                else
                {
                    tutorial_imagen.sprite = imagenDefecto;
                }
                ind++;
            }else
            {
                acabado = true;
            }
        }
        panelTutorial.SetParent(transform);
        panelTutorial.gameObject.SetActive(false);
        jugadorControlable = true;
    }

    void Update()
    {
        
        if(elegiendoColor && controlable)
        {

            Vector2 pos;
            if (Input.GetMouseButtonDown(0))
            {
                pos = Input.mousePosition;
                pos = rect_color.transform.InverseTransformPoint(pos);
                if (pos.x >= 0 && pos.x <= tamanyoColor.x  && 
                    pos.y >= 0 && pos.y <= tamanyoColor.y )
                        persigueRaton = true;
            }
            if(Input.GetMouseButtonUp(0))
            {

                persigueRaton = false;
                Debug.Log(persigueRaton);
                registraControl();
            }
            if (persigueRaton)
            {
                pos = Input.mousePosition;
                pos = rect_color.transform.InverseTransformPoint(pos);
                pos.y = 40;
                MoverMarca(pos);
                return;
            }
            pos = marca.rectTransform.localPosition;
               
            if (Input.GetKey(keys["izquierda"]))
            {
                pos.x -= velocidad * Time.unscaledDeltaTime;
            }
            if (Input.GetKey(keys["derecha"]))
            {
                pos.x += velocidad * Time.unscaledDeltaTime;
            }
            
            
            if (uicontrol.dosBotonModo)
            {
                pos.x += velocidad * Time.unscaledDeltaTime;
                if (pos.x > tamanyoColor.x)
                {
                    pos.x = 0;
                }
            }
            if(!pos.Equals(marca.rectTransform.localPosition))
            {
                MoverMarca(pos);
            }
            
            if(Input.GetKeyDown(keys["confirmar"]) || Input.GetKeyDown(keys["A"]))
            {
                cerrarElegirColor(true);
            }
            else if (Input.GetKeyDown(keys["cancelar"]) || Input.GetKeyDown(keys["B"]) || Input.GetMouseButtonUp(1))
            {
                cerrarElegirColor(false);
            }
        }
    }

    public IEnumerator cargarEscena()
    {
        yield return null;
        controlable = false;
        cargando = true;
        panelCargar.gameObject.SetActive(true);
        panelCargar.SetParent(canvasActual.transform);
        while(!escenaControlador.finalizado)
        {
            yield return null;
        }
        
        panelCargar.gameObject.SetActive(false);
        panelCargar.SetParent(this.transform);
        controlable = true;
        uiControlable = false;
        cargando = false;
        inputModule.desactivarRatonRegistrar = true;
        canvasActual = player.HUD;
    }

    public void cerrarElegirColor(bool confirmar)
    {
        registraControl();
        if(confirmar)
        {
            acabado(preview.color);
        }
        panelElegirColor.gameObject.SetActive(false);
        panelElegirColor.parent = transform;
        canNavegar = true;
        uiControlable = true;
        inputModule.desactivarRatonRegistrar = false;
        elegiendoColor = false;
        S_confirmarToggle();
    }

    public void Mappear()
    {
        inputModule.arriba = keys["arriba"];
        inputModule.abajo = keys["abajo"];
        inputModule.izquirda = keys["izquierda"];
        inputModule.derecha = keys["derecha"];
        inputModule.confirmar = keys["confirmar"];
        inputModule.cancelar = keys["cancelar"];
        inputModule.A = keys["A"];
        inputModule.B = keys["B"];
    }

    public bool uiControlable
    {
        get { return inputModule.controlable; }
        set { inputModule.controlable = value; }
    }
    public bool canNavegar
    {
        get { return !uicontrol.pausaNav; }
        set { uicontrol.pausaNav = !value; }
    }
    public void cambiarCursor(int tipo, int tamaño)
    {
        string prefijo;
        Vector2 hotspot = Vector2.zero; ;

        if (tipo == 0)
        {
            prefijo = "cursor_";
        }
        else
        {
            prefijo = "cursorEsp_";
            switch (tamaño)
            {
                case 0:
                    hotspot = new Vector2(4, 2);
                    break;
                case 1:
                    hotspot = new Vector2(6, 4);
                    break;
                case 2:
                    hotspot = new Vector2(8, 6);
                    break;
            }
        }
        Texture2D cursor = Resources.Load<Texture2D>(prefijo + tamaño);
        UnityEngine.Cursor.SetCursor(cursor, hotspot, CursorMode.ForceSoftware);
    }
    public bool jugadorControlable
    {
        get { if (player != null) return player.controlable;
            return false;
        }
        set { if(player != null) player.controlable = value; }
    }

    //1:ventana 0:completa
    public void PantallaCompleta(int modo)
    {
        if (modo == 0)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreen = true;
        }
        datosSistema.pantallaCompleta = modo;
    }

    IEnumerator PopUp(RectTransform rect)
    {
        float now = 0;
        bool antes_uiCanControl = uiControlable;
        bool antes_canNavegar = canNavegar;
        uiControlable = false;
        canNavegar = false;
        rect.anchoredPosition = Vector2.zero;
        yield return null;
        while(now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            rect.localScale = Vector3.one * (now / tiempoTransicion);
            yield return null;
        }
        rect.localScale = Vector3.one;
        uiControlable = antes_uiCanControl;
        canNavegar = antes_canNavegar;
    }
    public void ElegirColor(Vector2 pos,Action<Color> acabado)
    {
        canNavegar = false;
        uiControlable = false;
        inputModule.desactivarRatonRegistrar = true;
        panelElegirColor.gameObject.SetActive(true);
        panelElegirColor.parent = canvasActual.transform;
        panelElegirColor.localScale = Vector3.zero;
        rect_color.anchoredPosition = pos;
        StartCoroutine("PopUp",panelElegirColor);
        elegiendoColor = true ;
        mainCamera = Camera.main;
        tamanyoColor = new Vector2(tex_color.width, tex_color.height);
        MoverMarca(new Vector2(0,40));
        this.acabado = acabado;
    }
    void MoverMarca(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, 0, tamanyoColor.x);
        pos.y = Mathf.Clamp(pos.y, 0, tamanyoColor.y);
        
        marca.rectTransform.localPosition = pos;
        preview.color = tex_color.GetPixel((int)pos.x,(int)pos.y);
        float grayscale = 1- preview.color.grayscale;
        marca.color = new Color(grayscale, grayscale, grayscale);
    }
    public TMP_FontAsset getFont(int tipo)
    {
        if(tipo == 0)
        {
            if(tcm != null)
            {
                return tcm;
            }
            else
            {
                tcm = Resources.Load<TMP_FontAsset>("TCM");
                return tcm;
            }
        }
        else
        {
            if(openDyslexic != null)
            {
                return openDyslexic;
            }
            else
            {
                openDyslexic = Resources.Load<TMP_FontAsset>("OpenDyslexic");
                return openDyslexic;
            }
        }
        
    }

    public void CambiarModoSonido(int modo)
    {
        if(modo == 0)
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Stereo;
        }
        else
        {
            AudioSettings.speakerMode = AudioSpeakerMode.Mono;
        }

    }
    //0:ambiente 1: peligro 2:interaccion 3:interfaz 4:paso
    public void ModificarVolumen(int tipo,float vol)
    {
        float dB = (vol / 100 * 80) - 80;
        Debug.Log("soy controlador volumen es" + vol);
        switch(tipo)
        {
            case 0:
                audioMixer.SetFloat("AmbienteVol", dB);
                datosSistema.ambienteVol = vol;
                break;
            case 1:
                audioMixer.SetFloat("PeligroVol", dB);
                datosSistema.peligroVol = vol;
                break;
            case 2:
                audioMixer.SetFloat("InteraccionVol", dB);
                datosSistema.interaccionVol = vol;
                break;
            case 3:
                audioMixer.SetFloat("InterfazVol", dB);
                datosSistema.interfazVol = vol;
                break;
            case 4:
                audioMixer.SetFloat("PasoVol", dB);
                datosSistema.pasoVol = vol;
                break;
        }
    }

    public void CambiarControl(int tipo)
    {
        datosSistema.tipoControl = tipo;
        if(tipo == 2)
        {
            uicontrol.dosBotonModo = true;
            GameObject[] nav;
            if(canvasActual.getActualNavegable(out nav))
            {
                uicontrol.iniNavegacion(nav);
            }
        }
        else
        {
            uicontrol.dosBotonModo = false;
            uicontrol.LimpiarNav();
            eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        }
    }

    public void registraControl()
    {
        if (datosSistema.inputTime == 0 || !controlable)
            return;

        StartCoroutine("registrandoControl");
        
    }
    IEnumerator registrandoControl()
    {
        controlable = false;
        float now = 0;
        I_registrando.gameObject.SetActive(true);
        I_registrando.transform.parent = canvasActual.transform;
        while(now <= datosSistema.inputTime)
        {
            now += Time.unscaledDeltaTime;
            I_registrando. fillAmount = now / datosSistema.inputTime;
            yield return null;
        }
        I_registrando.rectTransform.parent = this.transform;
        I_registrando.gameObject.SetActive(false);
        controlable = true;
    }

    public bool cerrarJuego()
    {
        if (cerrar)
            return true;

        if (cerrandoJuego)
            return false;
        
        cerrandoJuego = true;
        TMP_FontAsset fuente = getFont(datosSistema.tipoFuente);

        panelSalir.gameObject.SetActive(true);
        foreach(TextMeshProUGUI text in salirTextos)
        {
            text.font = fuente;
        }
        uicontrol.apilarNavegacion(salirNav);
        panelSalir.parent = canvasActual.transform;
        ultControlable = controlable;
        if (player != null)
            ultJugadorControlable = jugadorControlable;
        ultUiControlable = uiControlable;
        controlable = true;
        jugadorControlable = false;
        uiControlable = true;
        StartCoroutine(PopUp(panelSalir));
        return false;
    }
    public void cerrarJuego(bool cerrar)
    {
        if(cerrar)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                this.cerrar = true;
                Application.Quit();
#endif
        }
        else
        {
            panelSalir.parent = transform;
            panelSalir.gameObject.SetActive(false);
            cerrandoJuego = false;
            uiControlable = ultUiControlable;
            uicontrol.cancelar();
            jugadorControlable = ultJugadorControlable;
            controlable = ultControlable;
            registraControl();

        }
    }

    public void cancelar()
    {
        if(uiControlable)
        {
            if (cerrandoJuego)
            {
                cerrarJuego(false);
                return;
            }
            if (!uicontrol.cancelar())
            {
                canvasActual.cancelar();
            }
            
        }
            
    }

    public void iniNavegacion(GameObject[] nav)
    {
        uicontrol.iniNavegacion(nav);
    }

    //---------------------Sonidos--------------------------

    public void S_jugar()
    {
        uisonido.Play_jugar();
    }
    public void S_cambioV()
    {
        uisonido.Play_cambioV();
    }
    public void S_cambioH()
    {
        uisonido.Play_cambioH();
    }
    public void S_confirmarToggle()
    {
        uisonido.Play_confirmarToggle();
    }
    public void S_cambioPagina()
    {
        uisonido.Play_cambioPagina();
    }
    public void S_salirBloque()
    {
        uisonido.Play_salirBloque();
    }
    public void S_musicaFondo(int id)
    {
        uisonido.Play_BGM(id);
    }
    public void S_pararMusica()
    {
        uisonido.Stop_BGM();
    }
    public void Set_cambioV(bool menuIni)
    {
        if(menuIni)
        {
            uisonido.cambioV = uisonido.cambioVMenuPrincipal;
        }
        else
        {
            uisonido.cambioV = uisonido.cambioVGeneral;
        }
    }
    //------------contraste controlador-------------------
    public void cambiarContraste(TipoContraste mascara)
    {
        if (mascara == TipoContraste.NADA)
            return;
        contrasteControlador = GameObject.Instantiate<ContrasteControlador>(contrastePrefab);
        if((TipoContraste.FONDO & mascara) == TipoContraste.FONDO)
        {
            if(datosSistema.opacidad_fondo == 0)
            {
                contrasteControlador.cambiarAlDefectoFondo();
            }
            else
            {
                contrasteControlador.cambiarContrasteFondo(datosSistema.color_fondo);
            }
        }
        if ((TipoContraste.PERSONAJE & mascara) == TipoContraste.PERSONAJE)
        {
            if (datosSistema.opacidad_personaje == 0)
            {
                contrasteControlador.cambiarAlDefectoPersonaje(datosJuego);
            }
            else
            {
                contrasteControlador.cambiarContrastePesonaje(datosSistema.color_personaje);
            }
        }
        if ((TipoContraste.INTERACTIVO & mascara) == TipoContraste.INTERACTIVO)
        {
            if (datosSistema.opacidad_interactivo == 0)
            {
                contrasteControlador.cambiarAlDefectoFondo();
            }
            else
            {
                contrasteControlador.cambiarContrasteInteractuable(datosSistema.color_interactivo);
            }
        }
        Destroy(contrasteControlador);
    }

}
