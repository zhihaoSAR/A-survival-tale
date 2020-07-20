using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiUiBoton : Button,ISelectHandler,IDeselectHandler,IPointerEnterHandler,IPointerExitHandler
{

    public RectTransform flecha;
    public RectTransform posicion;

    public override void OnSelect(BaseEventData eventData)
    {
        flecha.gameObject.SetActive(true);
        flecha.position = posicion.position;
        base.OnSelect(eventData);
    }
    public override void OnDeselect(BaseEventData data)
    {
        flecha.gameObject.SetActive(false);
        base.OnDeselect(data);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        flecha.gameObject.SetActive(true);
        flecha.position = posicion.position;
        base.OnPointerEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        flecha.gameObject.SetActive(false);
        base.OnPointerExit(eventData);
    }
}
