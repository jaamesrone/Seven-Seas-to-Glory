using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : EnemyClass
{
    // Implement the TakeDamage method to handle damage to the pirate
    public void TakeDamage(float damage)
    {
        // Reduce the pirate's health by the specified damage amount
        health -= damage;

        // Check if the pirate's health is below or equal to zero
        if (health <= 0f)
        {
            Die(); // Call a method to handle the pirate's death (e.g., play death animation, remove from the scene, etc.)
        }
    }

    // You can add additional methods for handling the pirate's death or any other behavior
    private void Die()
    {
        // Implement actions to be performed when the pirate dies
        Debug.Log("Pirate died!");
        Destroy(gameObject); // For simplicity, destroy the pirate GameObject, you might want a more sophisticated death sequence
    }
}
