using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raton : MonoBehaviour
{
    public Vector3 posicionEscena;
    public bool interactuable;
    public Interactuable objeto;
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }
    [SerializeField]
    LayerMask layerMask;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(posicionPantalla);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,100,layerMask))
            {
                if(hit.transform.TryGetComponent<Interactuable>(out objeto))
                {
                    interactuable = true;
                }
                else
                {
                    interactuable = false;
                    posicionEscena = hit.point;
                }
                
            }
            else
            {
                posicionEscena = transform.position;
            }
            
        }
    }


    public Vector3 posicionPantalla
    {
        get{ return Input.mousePosition; }
    }
}
