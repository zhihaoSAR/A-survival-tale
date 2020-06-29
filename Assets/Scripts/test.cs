using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform testObj,other;
    public Controlador c;
    public Pagina p;
    public Configuracion conf;
    Coroutine a = null;
    void Update()
    {
        /*
        Debug.DrawLine(testObj.transform.position, testObj.transform.position + testObj.transform.forward*5, Color.red);
        Vector3 perp = testObj.transform.forward;
        float x = perp.x;
        perp.x = -perp.z;
        perp.z = x;
        Debug.DrawLine(testObj.transform.position, testObj.transform.position+ perp*5, Color.green);
        */
        Vector3 forward = testObj.transform.TransformDirection(Vector3.forward);
        Vector3 toOther = other.position - testObj.transform.position;
        Debug.DrawLine(testObj.transform.position, testObj.transform.position + forward * 5, Color.red);
        Debug.DrawLine(testObj.transform.position, testObj.transform.position + toOther * 5, Color.green);
        if (Vector3.Dot(forward, toOther) < 0)
        {
            print("The other transform is behind me!");
        }
        else{
            print("front");
        }
        print(testObj.parent);


    }
    IEnumerator prueba()
    {
        int x = 0;
        Debug.Log("empiezo");
        while (x < 5)
        {
            x++;
            yield return null;
        }
        Debug.Log("acabado");
    }
}
