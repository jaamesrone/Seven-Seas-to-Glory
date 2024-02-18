using System.Collections;
using UnityEngine;

public class PirateSword : WeaponClass
{
    public BoxCollider swordCollider;

    private PlayerController playerController;

    private Pirate pirate;

    private void Start()
    {
        DisableAllSwordColliders();
        pirate = GetComponentInParent<Pirate>();
    }

    private void DisableAllSwordColliders()
    {
        PirateSword[] childSwords = GetComponentsInChildren<PirateSword>();
        foreach (PirateSword childSword in childSwords)
        {
            childSword.swordCollider.enabled = false;
        }
    }


    private void Update()
    {
        colliderToggle();
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if the collided object has a Player script on it.
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // reference the PlayerController script to grab that bool.
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null && playerController.isBlocking)
            {
                // reduce dmg if the player is blocking
                float reducedDamage = damage * 0.5f; // Deal half damage when blocking
                player.TakeDamage(reducedDamage);
                Debug.Log("how much dmg" + reducedDamage);
            }
            else
            {
                // if the player IS NOT blocking, take normal dmg.
                player.TakeDamage(damage);
                Debug.Log("how much dmg" + damage);
            }
        }
    }

    public void colliderToggle()
    {
        if (pirate != null && pirate.isAttacking)
        {
            swordCollider.enabled = true;
        }
        else
        {
            swordCollider.enabled = false;
        }
    }
}
