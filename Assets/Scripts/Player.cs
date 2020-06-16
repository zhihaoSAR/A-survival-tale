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
    float maxDis = 10;
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
    int contadorAtascado = 0;

    //-------------------estado de la maquina---------------
    enum Estado{PARADO,MOVER,PREPARARINTER,PARADOCONCAJA }
    Estado estado;
    public Action estadoActual;

    Vector3 destinoPrepararInt;

    public Action estadoParado,
            estadoMover,
            estadoPrepararInter,
            estadoParadoConCaja;


            //-----------------Acciones en cada estado-----------------------
    Func<bool> accionMover,
            accionCancelarMover,
            accionInteractuar,
            cancelarPreparar;

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
        estadoActual = estadoParado;
        //----------------------------------------------------------------
        ActualizarDatos();
    }

    
    public void ActualizarDatos()
    {
        agente.speed = 3.5f;
        playerCanControl = true;
        flecha.gameObject.SetActive(false);
        punto.gameObject.SetActive(false);
        if(datosSystema.tipoControl == 1)
        {
            accionMover = new Func<bool>(moverJugadorRaton);
            accionCancelarMover = new Func<bool>(paraJugadorRaton);
            accionInteractuar = new Func<bool>(interactuarRaton);
            cancelarPreparar = new Func<bool>(cancelarPrepararRaton);
        }
        else if(datosSystema.tipoControl == 0)
        {
            accionMover = new Func<bool>(moverJugadorWASD);
            accionCancelarMover = new Func<bool>(moverJugadorWASD);
            accionInteractuar = new Func<bool>(interactuarWASD);
            cancelarPreparar = new Func<bool>(cancelarPrepararWASD);
        }
        else if (datosSystema.tipoControl == 2)
        {
            accionMover = new Func<bool>(moverJugadorDosBoton);
            accionCancelarMover = new Func<bool>(paraJugadorDosBoton);
            accionInteractuar = new Func<bool>(interactuarDosBoton);
            cancelarPreparar = new Func<bool>(cancelarPrepararDosBoton);
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
            RecordarControl(false);
        }
        if(accionInteractuar())
        {
            estado = Estado.PREPARARINTER;
            estadoActual = estadoPrepararInter;
            RecordarControl(false);
        }
    }
    void ESTADO_MOVER()
    {
        if (accionCancelarMover())
        {
            estado = Estado.PARADO;
            estadoActual = estadoParado;
        }
    }
    void ESTADO_PREPARARINTER()
    {
        if(cancelarPreparar())
        {
            goto irEstadoParado;
        }
        
        if(agente.velocity.sqrMagnitude < 1)
        {
            contadorAtascado++;
            if ((transform.position-destinoPrepararInt).sqrMagnitude <= distanciaMaxInteractuable)
            {
                transform.position = destinoPrepararInt;
                estado = Estado.PARADOCONCAJA;
                objeto.setEstado();
            }
            if(contadorAtascado> 30)
            {
                goto irEstadoParado;
            }
        }
        else
        {
            contadorAtascado = 0;
        }
        if (agente.remainingDistance < distanciaLookAt)
        {
            transform.LookAt(objeto.transform);
        }
        return;
    irEstadoParado:
            agente.ResetPath();
            estado = Estado.PARADO;
            estadoActual = estadoParado;

    }
    void ESTADO_PARADOCONCAJA()
    {
        throw new NotImplementedException();
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
            if (distancia.magnitude < maxDis)
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

    bool cancelarPrepararRaton()
    {
        if(Input.GetMouseButtonDown(1))
        {
            return true;
        }
        return false;
    }
    bool cancelarPrepararWASD()
    {
        if (Input.GetKeyDown(datosSystema.keys["cancelar"]))
        {
            return true;
        }
        return false;
    }
    bool cancelarPrepararDosBoton()
    {
        if (Input.GetKeyDown(datosSystema.keys["B"]))
        {
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
                }
                
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            if(HUD.P_elegirDirCaja.gameObject.activeInHierarchy)
            {
                HUD.cerrarDireccion();
            }
        }
        return false;
    }
    bool interactuarWASD()
    {
        if (objeto != null && Input.GetKeyDown(datosSystema.keys["confirmar"]))
        {
            destinoPrepararInt = objeto.obtenerPosicion(transform);
            agente.SetDestination(destinoPrepararInt);
            estado = Estado.PREPARARINTER;
            estadoActual = estadoPrepararInter;
            return true;
        }
        return false;
    }
    bool interactuarDosBoton()
    {
        if (objeto != null && Input.GetKeyDown(datosSystema.keys["B"]))
        {
            destinoPrepararInt = objeto.obtenerPosicion(transform);
            agente.SetDestination(destinoPrepararInt);
            estado = Estado.PREPARARINTER;
            estadoActual = estadoPrepararInter;
            return true;
        }
        return false;
    }

    void preparaInteractuar(Vector3 posicion)
    {
        HUD.cerrarDireccion();
        agente.SetDestination(posicion);
        destinoPrepararInt = posicion;
        estado = Estado.PREPARARINTER;
        estadoActual = estadoPrepararInter;
        RecordarControl(false);
        
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
