using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateSword : WeaponClass
{

    // Called when the sword's hitbox collides with another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has an enemy script
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // Deal damage to the enemy
            player.TakeDamage(damage);

            Debug.Log("hitting player??");
        }
    }
}
