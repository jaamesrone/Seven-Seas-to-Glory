using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponClass
{

    // Called when the sword's hitbox collides with another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has an enemy script
        Pirate enemy = other.GetComponent<Pirate>();
        if (enemy != null)
        {
            // Deal damage to the enemy
            enemy.TakeDamage(damage);

            // Reduce durability
            ReduceDurability();


            Debug.Log("hitting pirate?");
        }
    }

    private void ReduceDurability()
    {
        // Reduce durability by a fixed amount per hit
        durability -= 2.5f;

        // Check if durability is below zero, indicating the sword is broken
        if (durability <= 0f)
        {
            Debug.Log("Sword is broken!");
            gameObject.SetActive(false);  
        }
    }
}
