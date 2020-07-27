using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class test : MonoBehaviour
{
    public PostProcessProfile profile;
    int mode = 0;
    void Start()
    {
        Debug.Log("dsafa");
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            ColorBlindCorrection colorBlindCorrection;
            profile.TryGetSettings<ColorBlindCorrection>(out colorBlindCorrection);
            if(mode < 3)
            {
                colorBlindCorrection.enabled.value = true;
                colorBlindCorrection.mode.value = mode;
            }
            else
                colorBlindCorrection.enabled.value = false;
            mode = ++mode % 4;
        }

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
