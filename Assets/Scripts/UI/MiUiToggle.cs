using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiUiToggle : Toggle,ISelectHandler,IDeselectHandler
{
    [SerializeField]
    public RectTransform flecha;
    [SerializeField]
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
}
