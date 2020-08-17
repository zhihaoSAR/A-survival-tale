using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleccionControl : Pagina
{
    public Pagina inicio;

    public void elegirControl(int tipo)
    {
        control.datosSistema.finalizadoConf = true;
        control.CambiarControl(tipo);
        menu.abrirPagina(inicio);
    }


}
