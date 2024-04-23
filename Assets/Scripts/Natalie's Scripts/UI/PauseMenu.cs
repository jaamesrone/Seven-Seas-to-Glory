using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public static bool ControlsUp = false;
    public static bool GameOver = false;
    public static bool inShop = false;
    public bool Shop = false;

    public GameObject pauseMenuUI;
    public GameObject controlsMenuUI;
    public GameObject GameOverUI;
    public Player player;
    public GameObject Ship;
    public GameObject shopUI;
    public GameObject[] shopNames;
    public AudioSource overworldAudioSource;
    public AudioSource oceanWaterAudioSource;
    public AudioSource pauseSFX;
    public AudioSource unpauseSFX;
    public AudioSource buttonSFX; // Add this line for the button sound effect

    void Start()
    {
        GamePaused = false;
        ControlsUp = false;
        GameOver = false;
        inShop = false;
        Shop = false;
        Time.timeScale = 1f;
    }

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
        if (player.health <= 0 || Ship.GetComponent<PlayerShipHealth>().health <= 0)
        {
            DisplayGameOver();
        }
        if (Shop && !inShop && Input.GetKeyDown(KeyCode.E))
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
        pauseMenuUI.SetActive(false);
        shopUI.SetActive(false);
        inShop = false;
        Time.timeScale = 1f;
        GamePaused = false;
        overworldAudioSource.volume = .5f;
        oceanWaterAudioSource.volume = .5f;
        unpauseSFX.Play();
    }

    void Pause()
    {
        SetPauseState();
        pauseMenuUI.SetActive(true);
        GamePaused = true;
        overworldAudioSource.volume = 0.05f;
        oceanWaterAudioSource.volume = 0.05f;
        pauseSFX.Play();
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
        overworldAudioSource.volume = 0.5f;
        oceanWaterAudioSource.volume = 0.5f;
        PlayButtonSFX(); // Play button sound effect
    }

    void DisplayGameOver()
    {
        SetPauseState();
        GameOverUI.SetActive(true);
        GameOver = true;
        overworldAudioSource.volume = 0.5f;
        oceanWaterAudioSource.volume = 0.5f;
        // Do not play the button sound effect when displaying game over screen
        // PlayButtonSFX(); 
    }

    public void Reset()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        PlayButtonSFX(); // Play button sound effect
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
        PlayButtonSFX(); // Play button sound effect
    }

    public void SetOceanAudioVolume(float volume)
    {
        oceanWaterAudioSource.volume = volume;
    }

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
        PlayButtonSFX(); // Play button sound effect
    }

    public void PlayButtonSFX()
    {
        buttonSFX.Play();
    }
}
