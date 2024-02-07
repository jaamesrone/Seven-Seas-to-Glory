using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public static bool ControlsUp = false;
    public static bool GameOver = false;
    //public static bool SettingsUp = false;

    public GameObject pauseMenuUI;
    public GameObject controlsMenuUI;
    public GameObject GameOverUI;
    public Player player;
    //public GameObject settingsMenuUI;

    public object ScreenManager { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOver)
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (player.health <= 0)
        {
            DisplayGameOver();
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controlsMenuUI.SetActive(false);
        ControlsUp = false;
        //settingsMenuUI.SetActive(false);
        //SettingsUp = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    void DisplayGameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
        GameOver = true;
    }

    //public void Reset()
    //{
     //   Time.timeScale = 1f;
     //   SceneManager.LoadSceneAsync("SevenSeasToGlory");
   // }

    public void Controls()
    {
        if (ControlsUp)
        {
            controlsMenuUI.SetActive(false);
            ControlsUp = false;
        }
        else
        {
            controlsMenuUI.SetActive(true);
            ControlsUp = true;
        }
    }

    /*
    public void Settings()
    {
        if (SettingsUp)
        {
            settingsMenuUI.SetActive(false);
            SettingsUp = false;
        }
        else
        {
            settingsMenuUI.SetActive(true);
            SettingsUp = true;
        }
    }
    */
    public void Menu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 1f;
        GamePaused = false;
        GameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}
