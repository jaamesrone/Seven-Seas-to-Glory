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
    public static bool inShop = false;
    public bool Shop = false;

    public GameObject pauseMenuUI;
    public GameObject controlsMenuUI;
    public GameObject GameOverUI;
    public Player player;
    public GameObject Ship;
    //public GameObject settingsMenuUI;
    public GameObject shopUI;
    public GameObject[] shopNames;

    public AudioSource resumeSound;
    public AudioSource menuSound;

    public object ScreenManager { get; private set; }

    void Start()
    {
        GamePaused = false;
        ControlsUp = false;
        GameOver = false;
        inShop = false;
        Shop = false;
        Time.timeScale = 1f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOver)
        {
            if (GamePaused)
            {
                menuSound.Play();
                Resume();
            }
            else
            {
                resumeSound.Play();
                Pause();
            }
        }
        if (player.health <= 0 || Ship.GetComponent<PlayerShipHealth>().health <= 0)
        {
            DisplayGameOver();
        }
        if(Shop && !inShop && Input.GetKeyDown(KeyCode.E))
        {
            DisplayShop();
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
        shopUI.SetActive(false);
        inShop = false;
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause()
    {
        SetPauseState();
        pauseMenuUI.SetActive(true);
        GamePaused = true;
    }

    void DisplayShop()
    {
        SetPauseState();
        int randIndex = Random.Range(0, shopNames.Length);
        for (int i = 0; i < shopNames.Length; i++)
        {
            shopNames[i].SetActive(i == randIndex);
        }
        shopUI.SetActive(true);
        inShop = true;
    }

    void DisplayGameOver()
    {
        SetPauseState();
        GameOverUI.SetActive(true);
        GameOver = true;
    }

    public void Reset()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

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

    private void SetPauseState()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

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
