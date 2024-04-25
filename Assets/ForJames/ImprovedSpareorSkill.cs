using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImprovedSpareorSkill : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject spareOrKillUI;

    public KillGain killGain;

    public float rotateSpeed = 50f;

    private bool aKeyPressed = false;
    private bool dKeyPressed = false;

    public AudioSource audioSource;
    public AudioClip activateSound;

    void Update()
    {
        object1.transform.Rotate(Vector3.forward, rotateSpeed * Time.unscaledDeltaTime);
        object2.transform.Rotate(Vector3.back, rotateSpeed * Time.unscaledDeltaTime);

        bool allActive = object1.activeSelf && object2.activeSelf && text1.activeSelf && text2.activeSelf && text3.activeSelf && spareOrKillUI.activeSelf;

        if (allActive)
        {
            if (!aKeyPressed && Input.GetKeyDown(KeyCode.A))
            {
                
                SetTextVisibility(text2, false);
                aKeyPressed = true;
                spareOrKillUI.SetActive(false);
                FindObjectOfType<FameMeter>().IncreaseStatus();
                PlayActivateSound();
                killGain.Spare();
            }

            if (!dKeyPressed && Input.GetKeyDown(KeyCode.D))
            {
                
                SetTextVisibility(text3, false);
                dKeyPressed = true;
                spareOrKillUI.SetActive(false);
                FindObjectOfType<FameMeter>().DecreaseStatus();
                PlayActivateSound();
                killGain.Kill();
            }
        }

        if (!spareOrKillUI.activeSelf && Input.GetKeyDown(KeyCode.K))
        {
            SetTextVisibility(text1, true);
            SetTextVisibility(text2, true);
            SetTextVisibility(text3, true);
            SetTextVisibility(spareOrKillUI, true);
            aKeyPressed = false;
            dKeyPressed = false;
            Time.timeScale = 0f; // Pause the game
        }

        if (Time.timeScale == 0f && Input.GetKeyDown(KeyCode.A))
        {
            Time.timeScale = 1f; // Unpause the game
        }

        if (Time.timeScale == 0f && Input.GetKeyDown(KeyCode.D))
        {
            Time.timeScale = 1f; // Unpause the game
        }
    }

    void SetTextVisibility(GameObject textObject, bool isVisible)
    {
        textObject.SetActive(isVisible);
    }

    void PlayActivateSound()
    {
        if (audioSource != null && activateSound != null)
        {
            audioSource.PlayOneShot(activateSound);
        }
    }
}