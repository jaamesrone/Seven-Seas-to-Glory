using UnityEngine;
using TMPro;

public class Sword : WeaponClass
{

    // Called when the sword's hitbox collides with another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has an enemy script
        Pirate enemy = other.GetComponent<Pirate>();
        SharkPirate sharkPirate = other.GetComponent<SharkPirate>();
        ImperialPirate royal = other.GetComponent<ImperialPirate>();
        SkeletonPirate zombie = other.GetComponent<SkeletonPirate>();

        if (enemy != null)
        {
            // Deal damage to the enemy
            enemy.TakeDamage(damage);

            // Reduce durability
            ReduceDurability();

            Debug.Log("Hitting pirate?");
        }
        else if (sharkPirate != null)
        {
            // Deal damage to the enemy
            sharkPirate.TakeDamage(damage);

            // Reduce durability
            ReduceDurability();

            Debug.Log("Hitting sharkpirate?");
        }
        else if (royal != null)
        {
            // Deal damage to the enemy
            royal.TakeDamage(damage);

            // Reduce durability
            ReduceDurability();

            Debug.Log("Hitting royal?");
        }
        else if (zombie != null)
        {
            // Deal damage to the enemy
            zombie.TakeDamage(damage);

            // Reduce durability
            ReduceDurability();

            Debug.Log("Hitting zombie?");
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
