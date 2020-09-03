//#define ESCENA_PRUEBA

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Controlador : MonoBehaviour
{
    public static Dictionary<string, KeyCode> keys;
    public MyInputModule inputModule;
    public DatosSistema datosSistema;
    public DatosJuego datosJuego;
    public UIControl uicontrol;
    static float tiempoTransicion = 0.2f;
    public EventSystem eventSystem;
    public UISonido uisonido;
    public EscenaControlador escenaControlador;
    [HideInInspector]
    public Player player;
    public CinemachineVirtualCamera camaraPrincipal;
    public bool controlable = true;
    

    public Menu canvaActual;
    public Configuracion configuracion;
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
    //--------------Animaciones de los efectos--------
    public AnimacionEfecto animEfec;
    //---------post processing--------------
    public PostProcessVolume volume;

    public static Controlador control;

    //--------panel morir-------------
    public RectTransform panelMuerto;
    bool muerto = false;

    static float proporcionX, proporcionY;
    public static Vector3 posicionRaton()
    {
        Vector3 pos = Input.mousePosition;
        pos.x *= 1920f / Screen.width;
        pos.y *= 1080f / Screen.height;
        return pos;
    }

    void Start()
    {
        control = this;
        SistemaGuardar.cargarDatosSistema(out datosSistema);
        Application.wantsToQuit += cerrarJuego;
        proporcionX = 1920f / Screen.width;
        proporcionY = 1080f / Screen.height;
        keys = datosSistema.keys;
        

#if ESCENA_PRUEBA
        uiControlable = false;
        inputModule.desactivarRatonRegistrar = true;

#else
        iniciarDatos();
        if(datosSistema.finalizadoConf)
            configuracion.abrirMenu(1);
        else
            configuracion.abrirMenu(0);
#endif
    }

    void iniciarDatos()
    {
        //--------control-------------
        Mappear();
        CambiarControl(datosSistema.tipoControl);
        //-----pantalla completa------
        PantallaCompleta(datosSistema.pantallaCompleta);
        //---------------cursor-----------
        cambiarCursor(datosSistema.tipoCursor, datosSistema.tamanyoCursor);
        //---------activar animacion----------
        ConfEfectoAnimacion();
        //--------color contraste----------
        TipoContraste mascara = TipoContraste.NADA;
        if(datosSistema.opacidad_fondo != 0)
        {
            mascara |= TipoContraste.FONDO;
        }
        if (datosSistema.opacidad_personaje != 0)
        {
            mascara |= TipoContraste.PERSONAJE;
        }
        if (datosSistema.opacidad_interactivo != 0)
        {
            mascara |= TipoContraste.INTERACTIVO;
        }
        cambiarContraste(mascara);
        //-----------Dicromatico-----------
        modoDicromatico(datosSistema.modoDicromatico);
        //--------modo sonido---------------
        CambiarModoSonido(datosSistema.audioModo);
        //---------volumen-----------------
        if(datosSistema.ambienteVol != 100)
        {
            ModificarVolumen(TipoVolumen.AMBIENTE, datosSistema.ambienteVol);
        }
        if (datosSistema.peligroVol != 100)
        {
            ModificarVolumen(TipoVolumen.PELIGRO, datosSistema.peligroVol);
        }
        if (datosSistema.interaccionVol != 100)
        {
            ModificarVolumen(TipoVolumen.INTERACCION, datosSistema.interaccionVol);
        }
        if (datosSistema.interfazVol != 100)
        {
            ModificarVolumen(TipoVolumen.INTERFAZ, datosSistema.interfazVol);
        }
        if (datosSistema.pasoVol != 100)
        {
            ModificarVolumen(TipoVolumen.PASO, datosSistema.pasoVol);
        }


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
        configuracion.cerrarMenu(0);
        escenaControlador.cargarEscenaIntermedio();
        escenaControlador.iniciarJuego(datosJuego);
    }
    public void modoDicromatico(int modo)
    {
        ColorBlindCorrection colorBlindCorrection;
        volume.profile.TryGetSettings<ColorBlindCorrection>(out colorBlindCorrection);
        if(modo == 0)
        {
            colorBlindCorrection.enabled.value = false;
        }
        else
        {
            colorBlindCorrection.enabled.value = true;
            colorBlindCorrection.mode.value = modo - 1;
        }
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
        panelTutorial.SetParent(configuracion.transform);
        panelTutorial.anchoredPosition3D = Vector3.zero;
        StartCoroutine(mostrarTutorial_Coroutine(imagenes, textos));

    }
    public bool esperaInput()
    {
        if (datosSistema.tipoControl == 0)
        {
            if (Input.GetKeyUp(keys["confirmar"]) || Input.GetKeyUp(keys["cancelar"]))
            {
                return true;
            }
        }
        if (datosSistema.tipoControl == 1)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                return true;
            }
        }
        if (datosSistema.tipoControl == 2)
        {
            if (Input.GetKeyUp(keys["A"]) || Input.GetKeyUp(keys["B"]))
            {
                return true;
            }
        }
        return false;
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
            if(esperaInput())
            {
                if (ind < textos.Length)
                {
                    tutorial_texto.text = textos[ind];
                    if (imagenes[ind] != null)
                    {
                        tutorial_imagen.sprite = imagenes[ind];
                    }
                    else
                    {
                        tutorial_imagen.sprite = imagenDefecto;
                    }
                    ind++;
                }
                else
                {
                    acabado = true;
                }
            }
        }
        panelTutorial.SetParent(transform);
        panelTutorial.gameObject.SetActive(false);
        jugadorControlable = true;
    }
    public void ConfEfectoAnimacion()
    {
        animEfec.ActualizaAnimacion(datosSistema.activarDecoracionAnim);
    }

    void Update()
    {
        
        if(elegiendoColor && controlable)
        {
            
            Vector2 pos;
            if (Input.GetMouseButtonDown(0))
            {
                pos = posicionRaton();
                pos -= rect_color.anchoredPosition;
                Debug.Log(pos);
                if (pos.x >= 0 && pos.x <= tamanyoColor.x  && 
                    pos.y >= 0 && pos.y <= tamanyoColor.y )
                        persigueRaton = true;
            }
            if(Input.GetMouseButtonUp(0))
            {

                persigueRaton = false;
                registraControl();
            }
            if (persigueRaton)
            {
                pos = posicionRaton();

                pos -= rect_color.anchoredPosition; ;
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

    public void iniciarPantallaCargar()
    {
        StartCoroutine(pantallaCargar());
    }
    IEnumerator pantallaCargar()
    {
        yield return null;
        controlable = false;
        cargando = true;
        panelCargar.gameObject.SetActive(true);
        panelCargar.SetParent(configuracion.transform);
        panelCargar.localRotation = Quaternion.identity;
        panelCargar.anchoredPosition3D = Vector3.zero;
        panelCargar.localScale = Vector3.one;
        while (cargando)
        {
            yield return null;
        }

        panelCargar.gameObject.SetActive(false);
        panelCargar.SetParent(this.transform);
        controlable = true;
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
        navegable = true;
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
    public bool navegable
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
        bool antes_controlable = controlable;
        bool antes_canNavegar = navegable;
        uiControlable = false;
        navegable = false;
        rect.localScale = Vector3.zero;
        rect.localRotation = Quaternion.identity;
        float tiempoInv = 1 / tiempoTransicion;
        yield return null;
        while(now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            rect.localScale = Vector3.one * (now * tiempoInv);
            yield return null;
        }
        rect.localScale = Vector3.one;
        controlable = antes_controlable;
        uiControlable = true;
        navegable = antes_canNavegar;
    }
    public void ElegirColor(Vector2 pos,Action<Color> acabado)
    {
        navegable = false;
        uiControlable = false;
        inputModule.desactivarRatonRegistrar = true;
        panelElegirColor.gameObject.SetActive(true);
        panelElegirColor.parent = configuracion.transform;
        panelElegirColor.localScale = Vector3.zero;
        panelElegirColor.anchoredPosition3D = Vector3.zero;

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
    public void ModificarVolumen(TipoVolumen tipo,float vol)
    {
        float dB = (vol / 100 * 80) - 80;
        Debug.Log("soy controlador volumen es" + vol);
        switch(tipo)
        {
            case TipoVolumen.AMBIENTE:
                audioMixer.SetFloat("AmbienteVol", dB);
                datosSistema.ambienteVol = vol;
                break;
            case TipoVolumen.PELIGRO:
                audioMixer.SetFloat("PeligroVol", dB);
                datosSistema.peligroVol = vol;
                break;
            case TipoVolumen.INTERACCION:
                audioMixer.SetFloat("InteraccionVol", dB);
                datosSistema.interaccionVol = vol;
                break;
            case TipoVolumen.INTERFAZ:
                audioMixer.SetFloat("InterfazVol", dB);
                datosSistema.interfazVol = vol;
                break;
            case TipoVolumen.PASO:
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
            if(configuracion.getActualNavegable(out nav))
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
        if(player != null)
        {
            player.actualizarControl();
        }
    }
    public void menuInicio()
    {
        iniciarPantallaCargar();
        StartCoroutine(volverMenuInicio());
    }
    IEnumerator volverMenuInicio()
    {
        yield return new WaitForSecondsRealtime(1);
        guardarDatoJuego();
        SceneManager.LoadScene(0);
    }
    public void registraControl()
    {
        if (!controlable)
            return;

        StartCoroutine("registrandoControl");
        
    }
    IEnumerator registrandoControl()
    {
        controlable = false;
        if(datosSistema.inputTime == 0)
        {
            yield return new WaitForEndOfFrame();
            goto final;
        }
        float now = 0;
        I_registrando.gameObject.SetActive(true);
        I_registrando.transform.parent = configuracion.transform;
        I_registrando.rectTransform.anchoredPosition3D = new Vector3(65, 65,0);
        I_registrando.rectTransform.localRotation = Quaternion.identity;
        while(now <= datosSistema.inputTime)
        {
            now += Time.unscaledDeltaTime;
            I_registrando. fillAmount = now / datosSistema.inputTime;
            yield return null;
        }
        I_registrando.rectTransform.parent = this.transform;
        I_registrando.gameObject.SetActive(false);
     final:
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
        panelSalir.SetParent(configuracion.transform);
        panelSalir.anchoredPosition3D = Vector3.zero;
        panelSalir.localRotation = Quaternion.identity;
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
            if(!cargando&&!muerto)
            {
                guardarDatoJuego();
                guardarDatoSistema();
            }
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
            eventSystem.SetSelectedGameObject(null);
            cerrandoJuego = false;
            uiControlable = ultUiControlable;
            uicontrol.cancelar();
            jugadorControlable = ultJugadorControlable;
            controlable = ultControlable;
            StartCoroutine(impedirControl());
            registraControl();

        }
    }
    IEnumerator impedirControl()
    {
        controlable = false;
        yield return new WaitForSecondsRealtime(0.1f);
        controlable = true;
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
                canvaActual.cancelar();
            }
            
        }
            
    }
    public void abrirConfiguracion()
    {

        canvaActual = configuracion;
        inputModule.desactivarRatonRegistrar = false;
        configuracion.abrirMenu(2);
    }
    public void cerrarConfiguracion()
    {
        canvaActual = player.HUD;
        GameObject[] navegables;
        canvaActual.getActualNavegable(out navegables);
        iniNavegacion(navegables);
        eventSystem.firstSelectedGameObject = navegables[1];
        inputModule.desactivarRatonRegistrar = true;
    }
    public void guardarDatoSistema()
    {
        SistemaGuardar.guardarDatosSistema(datosSistema);
        Debug.Log("guardando");
    }
    public void guardarDatoJuego()
    {
        if (player == null)
            return;
        datosJuego.nivelActual = escenaControlador.numNivelActual;
        if(escenaControlador.nivelActual != null)
        {
            datosJuego.niveles[datosJuego.nivelActual] = escenaControlador.nivelActual.generarConfig();
        }
        datosJuego.playerPos = player.transform.position;
        SistemaGuardar.guardarDatosJuego(datosJuego);

    }
    public void iniNavegacion(GameObject[] nav)
    {
        uicontrol.iniNavegacion(nav);
    }

    public void morir(string causa)
    {
        muerto = true;
        player.morir(causa);
        panelMuerto.gameObject.SetActive(true);
        panelMuerto.SetParent(configuracion.transform);
        panelMuerto.anchoredPosition3D = Vector3.zero;
        panelMuerto.localRotation = Quaternion.identity;
        StartCoroutine(morir_coroutine());
    }
    IEnumerator morir_coroutine()
    {
        float now = 0;
        controlable = false;
        float tiempoInv = 1 / tiempoTransicion;
        Image imagen = panelMuerto.GetComponent<Image>();
        yield return null;
        while (now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            Color color = imagen.color;
            color.a = 1 * (now * tiempoInv);
            imagen.color =color;
            yield return null;
        }
        bool esperandoInput = true;
        player.cambiarPersona();
        while(esperandoInput)
        {
            if(esperaInput())
            {
                esperandoInput = false;
            }
            yield return null;
        }
        controlable = true;
        panelMuerto.SetParent(transform);
        panelMuerto.gameObject.SetActive(false);
        escenaControlador.nivelActual.reintentarNivel();
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
