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
        control.pausaControl(true);
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
        control.pausaControl(false);
    }

    


}
