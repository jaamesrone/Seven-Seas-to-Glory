using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public AudioSource audioSource; // Assign your AudioSource component in the Inspector

    public void switchToMenu()
    {
        //StartCoroutine(PlaySFXAndWait("MainMenu"));
        SceneManager.LoadScene("MainMenu");
    }

    public void switchToGame()
    {
        //StartCoroutine(PlaySFXAndWait("SevenSeasToGlory"));
        SceneManager.LoadScene("SevenSeasToGlory");
    }

    public void switchToMenuControls()
    {
        //StartCoroutine(PlaySFXAndWait("MenuControlsScene"));
        SceneManager.LoadScene("MenuControlsScene");
    }

    public void switchToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        //StartCoroutine(PlaySFXAndWait(null));
        Application.Quit();
    }

    //private IEnumerator PlaySFXAndWait(string sceneName)
    //{
        //audioSource.Play();
        //yield return new WaitForSeconds(1f);

        //Cursor.lockState = sceneName != null ? CursorLockMode.Confined : CursorLockMode.Locked;
        //Cursor.visible = sceneName != null;

        //if (sceneName != null)
        //{
            //SceneManager.LoadScene(sceneName);
        //}
        //else
        //{
            //Application.Quit();
       // }
    //}
}
