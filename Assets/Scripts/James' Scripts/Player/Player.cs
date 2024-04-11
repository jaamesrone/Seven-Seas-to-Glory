using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerClass
{
    // Natalie's HealthBar
    public HealthBar healthBar;
    public AudioSource audioSource;
    public AudioClip lowHealthClip;
    private float maxHealth = 100f;
    public float recoveryRate = 5f;
    public float recoveryInterval = 2f;
    private bool isRecovering = false;

    private void Start()
    {
        // Initialize health
        health = maxHealth;
    }

    private void Update()
    {
        // Check if the 'O' key is pressed
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Set the player's health to 50
            SetHealth(25f);
        }

        // Start health recovery if health is below maxHealth and not already recovering
        if (health < maxHealth && !isRecovering)
        {
            isRecovering = true;
            StartCoroutine(RecoverHealth());
        }

        // Play low health sound if health is below or equal to 30
        if (health <= 30f)
        {
            PlayLowHealthSound();
        }
    }

    private IEnumerator RecoverHealth()
    {
        while (health < maxHealth)
        {
            yield return new WaitForSeconds(recoveryInterval);
            health += recoveryRate;
            health = Mathf.Min(health, maxHealth);
            healthBar.SetHealth(health);
        }

        isRecovering = false;
    }

    public void SetHealth(float newHealth)
    {
        health = Mathf.Clamp(newHealth, 0f, maxHealth);
        healthBar.SetHealth(health);
    }

    public void TakeDamage(float damage)
    {
        // player takes damage
        health -= damage;

        // More of Natalie's Healthbar
        healthBar.SetHealth(health);

        // Check if the player's health is below or equal to zero
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("player died!");
        // Destroy(gameObject);
    }

    private void PlayLowHealthSound()
    {
        if (lowHealthClip != null && !audioSource.isPlaying)
        {
            audioSource.clip = lowHealthClip;
            audioSource.Play();
        }
    }
}