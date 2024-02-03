using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void switchToMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
	Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
    public void switchToGame()
    {
	Cursor.lockState = CursorLockMode.Locked;
	Cursor.visible = false;
	SceneManager.LoadScene("SevenSeasToGlory");
    }
    public void switchToMenuControls()
    {
        Cursor.lockState = CursorLockMode.Confined;
	Cursor.visible = true;
        SceneManager.LoadScene("MenuControlsScene");
    }
    /*
    public void switchToGameControls()
    {
        Cursor.lockState = CursorLockMode.Confined;
	Cursor.visible = true;
        SceneManager.LoadScene("GameControlsScene");
    }
    public void switchToCSettings)
    {
        Cursor.lockState = CursorLockMode.Confined;
	Cursor.visible = true;
        SceneManager.LoadScene("SettingsScene");
    }
    public void switchToCredits()
    {
        Cursor.lockState = CursorLockMode.Confined;
	Cursor.visible = true;
        SceneManager.LoadScene("CreditsScene");
    }
*/
    public void Quit()
    {
        Application.Quit();
    }
}
