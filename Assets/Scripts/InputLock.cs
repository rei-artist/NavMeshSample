using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputLock : MonoBehaviour
{
    [SerializeField] private GameObject pauseText;
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
                pauseText.SetActive(false);
                Time.timeScale = 1f;
                paused = false;
                //Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if(!Judgment.isClear)
            {
                pauseText.SetActive(true);
                Time.timeScale = 0f;
               // Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                paused = true;
            }
        }
        if (paused || Judgment.isClear)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            }
        }
    }


    void OnApplicationQuit()
    {
        //Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
