using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitch : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    public AudioSource audioSource;

    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    public void switchToMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void switchToGame()
    {
        audioSource.Play();
        StartCoroutine(FadeAndSwitch("SevenSeasToGlory"));
    }

    public void switchToMenuControls()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("MenuControlsScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator FadeAndSwitch(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        Color originalColor = fadeImage.color;
        Color targetColor = new Color(0, 0, 0, 1);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            fadeImage.color = Color.Lerp(originalColor, targetColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;
        yield return new WaitForSeconds(1f); // Optional delay after fade
        SceneManager.LoadScene(sceneName);
    }
}