using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiUiBoton : Button,ISelectHandler,IDeselectHandler,IPointerEnterHandler,IPointerExitHandler
{

    public RectTransform flecha;
    public RectTransform posicion;

    public void OnSelect(BaseEventData eventData)
    {
        flecha.gameObject.SetActive(true);
        flecha.position = posicion.position;
        base.OnSelect(eventData);
    }
    public void OnDeselect(BaseEventData data)
    {
        flecha.gameObject.SetActive(false);
        base.OnDeselect(data);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        flecha.gameObject.SetActive(true);
        flecha.position = posicion.position;
        base.OnPointerEnter(eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        flecha.gameObject.SetActive(false);
        base.OnPointerExit(eventData);
    }
}
