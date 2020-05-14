using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    //---------------menus-----------------
    public RectTransform seleccionControl;


    //--------------variables------------
    [SerializeField]
    static float tiempoTransicion = 0.3f;
    public Controlador control;
    public DatosSistema datos;

    void Start()
    {
        datos = control.datosSistema;
    }

    public IEnumerator Transcicion(RectTransform entrante,RectTransform saliente,bool izqAder)
    {
        int dir;
        if(izqAder)
            dir = -1;
        else
            dir = 1;
        Vector2 dst = new Vector2( saliente.rect.width,0);
        dst.y = 0f;
        bool antes_uicanControl = control.uiCanControl;
        bool antes_canNavegar = control.canNavegar;
        control.uiCanControl = false ;
        control.canNavegar = false;
        entrante.anchoredPosition = saliente.anchoredPosition + dst*dir;
        float now = 0;
        Vector2 movimiento;
        Vector2 entranteOri= entrante.anchoredPosition,
                salienteOri = saliente.anchoredPosition;
        dir *= -1;
        
        while (now < tiempoTransicion)
        {
            now += Time.unscaledDeltaTime;
            movimiento = (now/tiempoTransicion)*dst*dir;
            entrante.anchoredPosition =entranteOri+ movimiento;
            saliente.anchoredPosition =salienteOri+ movimiento;
            yield return null;
        }
        movimiento = dst * dir ;
        entrante.anchoredPosition = entranteOri + movimiento;
        saliente.anchoredPosition = salienteOri + movimiento;
        control.uiCanControl = antes_uicanControl;
        control.canNavegar = antes_canNavegar;
    }

    


}
