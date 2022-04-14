using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool paused;

    private void Start()
    {
        pauseMenu.SetActive(false);
        paused = false;
    }
#pragma warning disable 0618
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.active);
            paused = !paused;
            //if (paused = true)
            //{
            //    Time.timeScale = 
            //}
            //DebugEx.Log("pauseMenu active: " + pauseMenu.active);
            //DebugEx.Log("pause bool: " + paused);

            //pauseMenu.active = !pauseMenu.active;
            if (pauseMenu.active && paused)
            {
                Time.timeScale = 0;
                PlayerControllerIsometric.canMove = false;
                //DebugEx.Log("paused");
            }
            if (/*Time.timeScale == 0 && */!pauseMenu.active && !paused)
            {
                Time.timeScale = 1;
                PlayerControllerIsometric.canMove = true;
                //DebugEx.Log("unpaused");
            }
        }
        //DebugEx.Log("TimeScale: " + Time.timeScale);
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        PlayerControllerIsometric.canMove = true;
        Time.timeScale = 1;
        paused = !paused;
    }
}
