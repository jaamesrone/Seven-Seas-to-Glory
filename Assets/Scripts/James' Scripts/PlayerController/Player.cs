using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerClass
{
    //Natalie's HealthBar
    public HealthBar healthBar;

    private void Start()
    {
        healthBar.SetMaxHealth(health);
    }

    public void TakeDamage(float damage)
    {
        // player takes dmg
        health -= damage;

        //More of Natalie's Healthbar
        healthBar.SetHealth(health);

        // Check if the pirate's health is below or equal to zero
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("player died!");
        Destroy(gameObject);
    }
}
