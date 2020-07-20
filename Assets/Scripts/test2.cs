using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    public void func1()
    {
        Debug.Log(GameObject.Find("Control").GetComponent<Controlador>());
    }
}
