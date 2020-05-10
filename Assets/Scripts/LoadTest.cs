using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTest : MonoBehaviour
{
    static bool once = false;
    Scene[] myscenes;
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKey(KeyCode.E) & !once)
        {
            once = true;
            //SceneManager.LoadSceneAsync("Scene2");
            SceneManager.LoadScene("Scene2", LoadSceneMode.Additive);
            myscenes = SceneManager.GetAllScenes();
        }
        if(Input.GetKey(KeyCode.B))
        {
            SceneManager.UnloadSceneAsync(myscenes[0]);
        }
    }
}
