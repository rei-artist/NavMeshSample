using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputLock : MonoBehaviour
{
    private bool paused = false;

    void Start()
    {
        //Cursor.visible = false;
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width/2, Screen.height/2));
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                Time.timeScale = 1f;
                paused = false;
                //Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Time.timeScale = 0f;
               // Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                paused = true;
            }
        }
    }


    void OnApplicationQuit()
    {
        //Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
