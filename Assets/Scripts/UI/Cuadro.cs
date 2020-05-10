using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cuadro : MonoBehaviour
{
    public GameObject[] navegable;
    [HideInInspector]
    public Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }


}
