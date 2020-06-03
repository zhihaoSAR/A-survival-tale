using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cuadro : MonoBehaviour,ISelectHandler,IDeselectHandler
{
    public GameObject[] navegable;
    [SerializeField]
    public RectTransform flecha;
    [SerializeField]
    public RectTransform posicion;
    [SerializeField]
    public Image image;
    

    public void OnSelect(BaseEventData eventData)
    {
        flecha.gameObject.SetActive(true);
        flecha.position = posicion.position;
    }
    public void OnDeselect(BaseEventData data)
    {
        flecha.gameObject.SetActive(false);
    }


}
