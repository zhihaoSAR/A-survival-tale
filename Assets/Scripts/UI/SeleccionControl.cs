using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleccionControl : Pagina
{

    public void elegirControl(int tipo)
    {
        control.CambiarControl(tipo);
    }


}
