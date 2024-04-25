using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTransition : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public Image image;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;
        Color color = image.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        color.a = 0.0f;
        image.color = color;

        image.enabled = false; // Disable the Image component
        gameObject.SetActive(false); // Deactivate the GameObject
    }
}