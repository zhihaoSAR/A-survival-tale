using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class LoadTest : MonoBehaviour
{
    static bool once = false;
    public NavMeshSurface surface;
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.E))
        {
            //SceneManager.LoadSceneAsync("Scene2");
            SceneManager.LoadScene("Scene2", LoadSceneMode.Additive);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.UnloadSceneAsync(3);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            surface.BuildNavMesh();
        }
    }
}
