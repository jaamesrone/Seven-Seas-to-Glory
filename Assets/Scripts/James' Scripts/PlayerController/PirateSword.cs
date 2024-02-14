using System.Collections;
using UnityEngine;

public class PirateSword : WeaponClass
{
    public BoxCollider swordCollider;

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
        // Check if the collided object has an enemy script
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // Deal damage to the enemy
            player.TakeDamage(damage);
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
