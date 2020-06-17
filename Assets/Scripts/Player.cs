using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    //-------------------dos boton--------------
    public Transform flecha, punto;
    float altura = 15;
    bool seleccionaDir = true;
    float velNavGirar = 60f, velNavMover = 6;
    float sqrMaxDis = 100;
    Vector3 dir;
    //------------------raton--------------
    [SerializeField]
    Raton manejadorRaton;
    //-------------------------------------------------
    [HideInInspector]
    public Controlador control;
    DatosSistema datosSystema;
    [SerializeField]
    NavMeshAgent agente;
    Camera mainCamera;
    public bool playerCanControl;
    [SerializeField]
    CanvaJuego HUD;
    [HideInInspector]
    public Interactuable objeto;
    [HideInInspector]
    public float sqrDisObjeto = 100;

    public float distanciaLookAt = 2;
    //distancia cuadrado
    public float distanciaMaxInteractuable = 0.01f;
    //timepoAtascado
    float contadorAtascado = 0;

    //-------------------estado de la maquina---------------
    public enum Estado{PARADO,MOVER,PREPARARINTER,PARADOCONCAJA,MOVERCONCAJA }
    [HideInInspector]
    public Estado estado;
    public Action estadoActual;

    Vector3 destinoPrepararInt;

    [HideInInspector]
    public Vector3 dirMoverCaja;
    public float velMoverCaja = 3;
    [HideInInspector]
    public float disPendiente;
    Coroutine Coroutine_moverConCaja;

    public Action estadoParado,
            estadoMover,
            estadoPrepararInter,
            estadoParadoConCaja,
            estadoMoverConCaja;


            //-----------------Acciones en cada estado-----------------------
    Func<bool> accionMover,
            accionCancelarMover,
            accionInteractuar,
            accionCancelar,
            accionMoverConCaja;

    void Start()
    {
        control = Controlador.control;
        control.player = this;
        estado = Estado.PARADO;
        datosSystema = control.datosSistema;
        mainCamera = Camera.main;
        //---------------asignar funcion a estado-------------------------
        estadoParado = new Action(ESTADO_PARADO);
        estadoMover = new Action(ESTADO_MOVER);
        estadoPrepararInter = new Action(ESTADO_PREPARARINTER);
        estadoParadoConCaja = new Action(ESTADO_PARADOCONCAJA);
        estadoMoverConCaja = new Action(ESTADO_MOVERCONCAJA);
        estadoActual = estadoParado;
        //----------------------------------------------------------------
        ActualizarDatos();
    }

    
    public void ActualizarDatos()
    {
        manejadorRaton.enabled = false;
        agente.speed = 3.5f;
        playerCanControl = true;
        flecha.gameObject.SetActive(false);
        punto.gameObject.SetActive(false);
        if(datosSystema.tipoControl == 1)
        {
            manejadorRaton.enabled = true;
            accionMover = new Func<bool>(moverJugadorRaton);
            accionCancelarMover = new Func<bool>(paraJugadorRaton);
            accionInteractuar = new Func<bool>(interactuarRaton);
            accionCancelar = new Func<bool>(cancelarRaton);
            accionMoverConCaja = new Func<bool>(accionMoverConCajaRaton);
        }
        else if(datosSystema.tipoControl == 0)
        {
            accionMover = new Func<bool>(moverJugadorWASD);
            accionCancelarMover = new Func<bool>(moverJugadorWASD);
            accionInteractuar = new Func<bool>(interactuarWASD);
            accionCancelar = new Func<bool>(cancelarWASD);
            accionMoverConCaja = new Func<bool>(accionMoverConCajaWASD);
        }
        else if (datosSystema.tipoControl == 2)
        {
            accionMover = new Func<bool>(moverJugadorDosBoton);
            accionCancelarMover = new Func<bool>(paraJugadorDosBoton);
            accionInteractuar = new Func<bool>(interactuarDosBoton);
            accionCancelar = new Func<bool>(cancelarDosBoton);
            accionMoverConCaja = new Func<bool>(accionMoverConCajaDosBoton);
        }
    }

    void Update()
    {
        if (!control.canControl || !playerCanControl)
            return;
        actualizarObjDis();
        estadoActual();
        

    }
    void actualizarObjDis()
    {
        if (objeto != null)
            sqrDisObjeto = (objeto.transform.position - transform.position).sqrMagnitude;
        else
            sqrDisObjeto = 100;
    }
    void RecordarControl(bool activar)
    {
        if(HUD.recordando != activar)
        {
            HUD.MostrarControlInteractuar(activar);
        }
    }
    void ESTADO_PARADO()
    {
        RecordarControl(objeto != null);
        if (accionMover())
        {
            estado = Estado.MOVER;
            estadoActual = estadoMover;
            desactivarSenyales();
        }
        if(accionInteractuar())
        {
            estado = Estado.PREPARARINTER;
            estadoActual = estadoPrepararInter;
            RecordarControl(false);
            desactivarSenyales();
        }
    }
    void ESTADO_MOVER()
    {
        RecordarControl(objeto != null);
        if (accionCancelarMover())
        {
            estado = Estado.PARADO;
            estadoActual = estadoParado;
        }
    }
    void ESTADO_PREPARARINTER()
    {
        agente.destination = destinoPrepararInt;
        if (accionCancelar())
        {
            goto irEstadoParado;
        }
        if (agente.remainingDistance < distanciaLookAt)
        {
            transform.LookAt(objeto.transform);
        }
        if (agente.velocity.sqrMagnitude < 0.64f)
        {
            contadorAtascado+= Time.deltaTime;
            if ((transform.position-destinoPrepararInt).sqrMagnitude <= distanciaMaxInteractuable)
            {
                transform.position = destinoPrepararInt;
                estado = Estado.PARADOCONCAJA;
                objeto.finPreparar();
                agente.ResetPath();
            }
            
            if(contadorAtascado> 1f)
            {
                goto irEstadoParado;
            }
        }
        else
        {
            contadorAtascado = 0;
        }
        
        return;
    irEstadoParado:
            agente.ResetPath();
            estado = Estado.PARADO;
            estadoActual = estadoParado;

    }
    void ESTADO_PARADOCONCAJA()
    {
        if (accionMoverConCaja())
        {
            Coroutine_moverConCaja = StartCoroutine(MoverCaja());
            estado = Estado.MOVERCONCAJA;
            estadoActual = estadoMoverConCaja;
            desactivarSenyales();
        }
        if (accionCancelar())
        {
            objeto.finInteractuar();
            objeto = null;
            estado = Estado.PARADO;
            estadoActual = estadoParado;
            desactivarSenyales();
        }
        
    }
    void ESTADO_MOVERCONCAJA()
    {
        if (Coroutine_moverConCaja == null || accionCancelar())
        {
            StopCoroutine(Coroutine_moverConCaja);
            Coroutine_moverConCaja = null;
            estado = Estado.PARADOCONCAJA;
            estadoActual = estadoParadoConCaja;
        }
        accionMoverConCaja();
    }

    bool moverJugadorRaton()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(!manejadorRaton.interactuable)
            {
                agente.SetDestination(manejadorRaton.posicionEscena);
                if (HUD.P_elegirDirCaja.gameObject.activeInHierarchy)
                    HUD.cerrarDireccion();
                control.registraControl();
                return true;
            }
            
        }
        return false;
    }
    bool moverJugadorWASD()
    {
        float horizontal = 0, vertical = 0;
        if (Input.GetKey(datosSystema.keys["arriba"]))
            vertical += 1;
        if (Input.GetKey(datosSystema.keys["abajo"]))
            vertical -= 1;
        if (Input.GetKey(datosSystema.keys["izquierda"]))
            horizontal -= 1;
        if (Input.GetKey(datosSystema.keys["derecha"]))
            horizontal += 1;
        if (vertical != 0 || horizontal != 0)
        {
            Vector3 dir = (new Vector3(horizontal, 0, vertical)).normalized;
            dir = rotacionCamara * dir;
            agente.destination = transform.position + dir * 0.7f;
            return estado.Equals(Estado.PARADO);
        }
        return estado.Equals(Estado.MOVER);
    }
    bool moverJugadorDosBoton()
    {
        if (seleccionaDir)
        {
            if (!flecha.gameObject.activeInHierarchy)
            {
                flecha.gameObject.SetActive(true);
                flecha.transform.position = transform.position;
            }
            if (Input.GetKeyDown(datosSystema.keys["A"]))
            {
                dir = Quaternion.Euler(0, flecha.transform.eulerAngles.y, 0) * Vector3.right;
                seleccionaDir = false;
                flecha.gameObject.SetActive(false);
                control.registraControl();
            }
            flecha.transform.eulerAngles = new Vector3(-90,
                                        flecha.transform.eulerAngles.y + velNavGirar * Time.deltaTime,
                                        0);
            return false;
        }
        else
        {
            if (!punto.gameObject.activeInHierarchy)
            {
                punto.gameObject.SetActive(true);
                goto resetearPos;

            }
            if (Input.GetKeyDown(datosSystema.keys["A"]))
            {
                Ray ray = new Ray(punto.transform.position, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    agente.SetDestination(hit.point);
                }
                seleccionaDir = true;
                punto.gameObject.SetActive(false);
                control.registraControl();
                return true;
            }
            if (Input.GetKeyDown(datosSystema.keys["B"]))
            {
                seleccionaDir = true;
                punto.gameObject.SetActive(false);
                control.registraControl();
            }
            Vector3 distancia = punto.transform.position - transform.position;
            distancia.y = 0;
            if (distancia.sqrMagnitude < sqrMaxDis)
                punto.transform.Translate(dir * velNavMover * Time.deltaTime, Space.World);
            else
                goto resetearPos;

            return false;
        resetearPos:
            punto.transform.localPosition = Vector3.up * altura+transform.position;
            return false;
        }
    }

    bool paraJugadorRaton()
    {
        if (agente.remainingDistance <= agente.stoppingDistance)
        {
            goto resetear;
        }
        if (Input.GetMouseButtonUp(1))
        {
            control.registraControl();
            goto resetear;
        }
        moverJugadorRaton();
        return false;

        resetear:
            agente.ResetPath();
            return true;
    }
    bool paraJugadorDosBoton()
    {
        if (agente.remainingDistance <= agente.stoppingDistance)
        {
            goto resetear;
        }
        if (Input.GetKeyDown(datosSystema.keys["B"]))
        {
            control.registraControl();
            goto resetear;
        }
        return false;

    resetear:
        agente.ResetPath();
        return true;
    }

    bool cancelarRaton()
    {
        if(Input.GetMouseButtonDown(1))
        {
            control.registraControl();
            return true;
        }
        return false;
    }
    bool cancelarWASD()
    {
        if (Input.GetKeyDown(datosSystema.keys["cancelar"]))
        {
            control.registraControl();
            return true;
        }
        return false;
    }
    bool cancelarDosBoton()
    {
        if (Input.GetKeyDown(datosSystema.keys["B"]))
        {
            control.registraControl();
            return true;
        }
        return false;
    }

    bool interactuarRaton()
    {
        if(manejadorRaton.interactuable && Input.GetMouseButtonDown(0))
        {
            if(manejadorRaton.objeto.tipo.Equals(Interactuable.TipoInteractuable.CAJA))
            {
                if(!HUD.P_elegirDirCaja.gameObject.activeInHierarchy)
                {
                    objeto = manejadorRaton.objeto;
                    Caja caja = manejadorRaton.objeto as Caja;
                    HUD.mostrarDireccion(manejadorRaton.posicionPantalla, caja.transform, rotacionCamara,
                                        caja.obtenerPosiciones(), new Action<Vector3>(preparaInteractuar));
                    control.registraControl();
                }
                
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            if(HUD.P_elegirDirCaja.gameObject.activeInHierarchy)
            {
                objeto = null;
                HUD.cerrarDireccion();
                control.registraControl();
            }
        }
        return false;
    }
    bool interactuarWASD()
    {
        if (objeto != null && Input.GetKeyDown(datosSystema.keys["confirmar"]))
        {
            destinoPrepararInt = objeto.obtenerPosicion(transform);
            estado = Estado.PREPARARINTER;
            estadoActual = estadoPrepararInter;
            control.registraControl();
            return true;
        }
        return false;
    }
    bool interactuarDosBoton()
    {
        if (objeto != null && Input.GetKeyDown(datosSystema.keys["B"]))
        {
            destinoPrepararInt = objeto.obtenerPosicion(transform);
            estado = Estado.PREPARARINTER;
            estadoActual = estadoPrepararInter;
            control.registraControl();
            return true;
        }
        return false;
    }

    void preparaInteractuar(Vector3 posicion)
    {
        HUD.cerrarDireccion();
        destinoPrepararInt = posicion;
        estado = Estado.PREPARARINTER;
        estadoActual = estadoPrepararInter;
        RecordarControl(false);
        control.registraControl();

    }

    bool accionMoverConCajaRaton()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Caja caja = objeto as Caja;
            Vector3 objetivo = (manejadorRaton.posicionEscena - transform.position);
            objetivo = Vector3.Project(objetivo, caja.dir);
            int empujar = Vector3.Dot(caja.dir, objetivo.normalized) < 0?-1:1;
            if (empujar < 0)
            {
                if (!caja.conAsa)
                {
                    disPendiente = 0;
                    return false;
                }
                    
            }
            dirMoverCaja = caja.dir * empujar;
            disPendiente = objetivo.magnitude;
            control.registraControl();
            return true;
        }

        return false;
    }
    bool accionMoverConCajaWASD()
    {
        Caja caja = objeto as Caja;
        Vector3 objetivo = Vector3.zero;
        if (Input.GetKey(datosSystema.keys["arriba"]))
        {
            objetivo.z+=1;
        }
        if (Input.GetKey(datosSystema.keys["derecha"]))
        {
            objetivo.x += 1;
        }
        if (Input.GetKey(datosSystema.keys["abajo"]))
        {
            objetivo.z -= 1;
        }
        if (Input.GetKey(datosSystema.keys["izquierda"]))
        {
            objetivo.x -= 1;
        }
        
        if (!objetivo.Equals(Vector3.zero))
        {
            objetivo = rotacionCamara * objetivo;
            if (Vector3.Dot(caja.dir, objetivo.normalized) >= 0)
            {
                objetivo = caja.dir;
            }
            else
            {
                if (!caja.conAsa)
                {
                    disPendiente = 0;
                    return false;
                }
                objetivo = -caja.dir;
            }
            dirMoverCaja = objetivo;
            disPendiente = (objetivo*velMoverCaja*Time.deltaTime).magnitude;
            return true;
        }

        return false;
    }
    bool accionMoverConCajaDosBoton()
    {
        if (!estado.Equals(Estado.PARADOCONCAJA))
            return false;
        Caja caja = objeto as Caja;
        Vector3 objetivo = Vector3.zero;
        if (!punto.gameObject.activeInHierarchy)
        {
            punto.gameObject.SetActive(true);
            goto resetearPos;
            

        }
        if (Input.GetKeyDown(datosSystema.keys["A"]))
        {
            objetivo = punto.transform.position-transform.position;
            punto.gameObject.SetActive(false);
            objetivo.y = 0;
            if (Vector3.Dot(caja.dir, objetivo.normalized) >= 0)
            {
                dirMoverCaja = caja.dir;
            }
            else
            {
                dirMoverCaja = -caja.dir;
            }
            disPendiente = objetivo.magnitude;
            control.registraControl();
            return true;
        }
        
        Vector3 distancia = punto.transform.position - transform.position;
        distancia.y = 0;
        if (distancia.sqrMagnitude <= sqrMaxDis)
            punto.transform.Translate(caja.dir * velNavMover * Time.deltaTime, Space.World);
        else
            goto resetearPos;

        return false;
    resetearPos:
        if(caja.conAsa)
            punto.transform.localPosition = Vector3.up * altura + (transform.position-(caja.dir *Mathf.Sqrt( sqrMaxDis)));
        else
        {
            punto.transform.localPosition = Vector3.up * altura + transform.position;
        }
        return false;

    }

    void desactivarSenyales()
    {
        flecha.gameObject.SetActive(false);
        punto.gameObject.SetActive(false);
    }

    IEnumerator MoverCaja()
    {
        Vector3 cajaUltPos = objeto.transform.position;
        Vector3 miUltPos = transform.position;
        yield return null;
        while(disPendiente > 0.01)
        {
            if (!playerCanControl || !control.canControl)
                yield return null;
            Vector3 movimiento = (dirMoverCaja * velMoverCaja * Time.deltaTime);
            float distanciaMovido = movimiento.magnitude;
            transform.Translate(movimiento,Space.World);
            Vector3 miNuevoPos =transform.position;
            Vector3 cajaNuevoPos = objeto.transform.position;
            if ((cajaUltPos - cajaNuevoPos).sqrMagnitude < movimiento.sqrMagnitude-0.0001)
            {
                objeto.transform.parent = null;
                transform.position = objeto.obtenerPosicion(transform);
                objeto.transform.parent = transform;
                break;
            }
            if ((miUltPos - miNuevoPos).sqrMagnitude < movimiento.sqrMagnitude - 0.0001)
            {
                objeto.transform.parent = null;
                transform.position = objeto.obtenerPosicion(transform);
                objeto.transform.parent = transform;
                break;
            }
            disPendiente -= distanciaMovido;
            if(disPendiente < 0.0001)
            {
                break;
            }

            miUltPos = miNuevoPos;
            cajaUltPos = cajaNuevoPos;
            yield return null;
        }
        Coroutine_moverConCaja = null;
        
    }

    public void asigObjeto(Interactuable objeto)
    {
        if (estado.Equals(Player.Estado.PARADO) || estado.Equals(Estado.MOVER))
        {
            if (this.objeto != null && manejadorRaton.enabled == true)
            {
                return;
            }
            if ((transform.position - objeto.transform.position).sqrMagnitude
            < sqrDisObjeto)
            {
                this.objeto = objeto;
            }
        }
    }
    public void desasigObjeto(Interactuable objeto)
    {
        if(estado.Equals(Estado.PARADO) || estado.Equals(Estado.MOVER))
        {
            if (this.objeto.Equals(objeto))
            {
                this.objeto = null;
            }
        }
        
    }

    public Quaternion rotacionCamara
    {
        get
        {
            Vector3 ori = mainCamera.transform.forward;
            ori.y = ori.z;
            ori.z = 0;
            if (mainCamera.transform.up.y < 0)
            {
                ori.y = -ori.y;
            }
            return Quaternion.FromToRotation(ori, Vector3.up);
        }
    }
    


}
