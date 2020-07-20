using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Nivel1 : Tutorial
{
    public string nombre;
    Dictionary<string, string> textos;
    public Sprite[] imagenes;
    public override void mostrarTutorial(int id)
    {
        Controlador control = Controlador.control;
        DatosSistema datos = control.datosSistema;
        if(id == 0)
        {
            if(datos.tipoControl == 0)
            {
                control.mostrarTutorial(new Sprite[1], new string[1] { textos["moverWASD"] });
                return;
            }
            if(datos.tipoControl == 1)
            {
                control.mostrarTutorial(new Sprite[1], new string[1] { textos["moverRaton"] });
                return;
            }
            if (datos.tipoControl == 2)
            {
                control.mostrarTutorial(new Sprite[2] { imagenes[0],imagenes[1]}, new string[2] { textos["moverDBDir"], textos["moverDBDis"] });
                return;
            }
        }
        if (id == 1)
        {
            if (datos.tipoControl == 0)
            {
                control.mostrarTutorial(new Sprite[2] { imagenes[3],null},
                    new string[2] { textos["interactuarWASDoDB"], textos["moverCajaWASD"] });
                return;
            }
            if (datos.tipoControl == 1)
            {
                control.mostrarTutorial(new Sprite[2] { imagenes[2], null },
                    new string[2] { textos["interactuarRaton"], textos["moverCajaRaton"] });
                return;
            }
            if (datos.tipoControl == 2)
            {
                control.mostrarTutorial(new Sprite[2] { imagenes[4], null },
                    new string[2] { textos["interactuarWASDoDB"], textos["moverCajaDB"] });
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cargarTexto();
    }
    void cargarTexto()
    {
        textos = new Dictionary<string, string>();
        textos.Add("moverRaton", "Clicar al suelo para mover");
        textos.Add("moverWASD", "Usar las teclas de direcciones para mover");
        textos.Add("moverDBDir", "Pulsa boton de confirmar para elegir dirección");
        textos.Add("moverDBDis", "El punto rojo representa donde quieres ir\nPulsa boton confirmara para mover\nPulsa boton cancelar para volver elegir la dirección");
        textos.Add("interactuarRaton", "Clicar caja para elegir la dirección donde quieres empujar");
        textos.Add("moverCajaRaton", "Una vez elegido la direccion clicar la posición para ir mover la caja\n Sólo la parte con Asa puede tirar la caja\nPulsa boton derecho para dejar de interactuar");
        textos.Add("interactuarWASDoDB", "Cuando aparece este icono,pulsa cancelar para interactuar con los objetos\n La personaje se interactua con el lado más cerca");
        textos.Add("moverCajaWASD", "Usar las teclas de direcciones para mover,Sólo la parte con Asa puede tirar la caja\nPulsa boton cancelar para dejar interactuar");
        textos.Add("moverCajaDB", "El punto representa donde quieres mover la caja\nPulsa boton cancelar para dejar interactuar");

    }
}
