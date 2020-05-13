using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public RectTransform entrante;
    public RectTransform saliente;
    public Texture2D cursorped;
    public Texture2D cursormed;
    public Texture2D cursorgran;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Cursor.SetCursor(cursorped, new Vector2(4,2), CursorMode.ForceSoftware);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Cursor.SetCursor(cursormed, new Vector2(6, 4), CursorMode.ForceSoftware);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Cursor.SetCursor(cursorgran, new Vector2(8, 6), CursorMode.ForceSoftware);
        }
    }
}
