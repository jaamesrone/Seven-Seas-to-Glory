using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider staminaSlider;
    public AudioSource audioSource;

    public AudioClip halfStaminaSFX;
    public AudioClip lowStaminaSFX;
    public AudioClip outOfStaminaSFX;

    private bool playedHalfSFX;
    private bool playedLowSFX;

    private void Start()
    {
        playedHalfSFX = false;
        playedLowSFX = false;
    }

    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        float percentage = currentStamina / maxStamina;
        staminaSlider.value = percentage;

        if (percentage <= 0.5f && !playedHalfSFX)
        {
            audioSource.PlayOneShot(halfStaminaSFX);
            playedHalfSFX = true;
        }

        if (percentage <= 0.15f && !playedLowSFX)
        {
            audioSource.PlayOneShot(lowStaminaSFX);
            playedLowSFX = true;
        }

        if (percentage <= 0f)
        {
            audioSource.PlayOneShot(outOfStaminaSFX);
            // Add logic to handle player's behavior when out of stamina
        }
    }
}
