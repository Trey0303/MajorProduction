using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }
#pragma warning disable 0618
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.active);

            //pauseMenu.active = !pauseMenu.active;
        }
        if (pauseMenu.active)
        {
            Time.timeScale = 0;
            PlayerControllerIsometric.canMove = false;
        }
        else if(Time.timeScale == 0 && !pauseMenu.active)
        {
            Time.timeScale = 1;
            PlayerControllerIsometric.canMove = true;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }
}
