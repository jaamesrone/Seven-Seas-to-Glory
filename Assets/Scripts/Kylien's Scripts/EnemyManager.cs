using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemySpawner enemySpawner; // Reference to the EnemySpawner script

    public GameObject pauseText; // Reference to the pause text object
    private bool isPaused = false; // Flag to track if the game is paused

    void Update()
    {
        // Check if there is only 1 enemy remaining
        if (enemySpawner.numberOfEnemies == 1)
        {
            // Pause the game and display the text
            if (!isPaused)
            {
                Time.timeScale = 0f; // Pause the game
                pauseText.SetActive(true); // Show the pause text
                isPaused = true; // Update the flag
            }

            // Check for input to kill or spare the enemy
            if (Input.GetKeyDown(KeyCode.A))
            {
                DestroyLastEnemy();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Implement sparing the enemy here
                // For example, you could disable the pause text and resume the game
                // pauseText.SetActive(false);
                // Time.timeScale = 1f;
                // isPaused = false;
            }
        }
    }

    private void DestroyLastEnemy()
    {
        // Find the last remaining enemy in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Ensure there is at least one enemy in the array
        if (enemies.Length > 0)
        {
            // Destroy the last enemy in the array
            Destroy(enemies[enemies.Length - 1]);
        }

        // Reset the game state if there are no more enemies
        if (enemies.Length == 1)
        {
            // You can implement game reset logic here
            // For example, reset the score, level, etc.
        }

        // Resume the game and hide the pause text
        Time.timeScale = 1f;
        pauseText.SetActive(false);
        isPaused = false;
    }
}
