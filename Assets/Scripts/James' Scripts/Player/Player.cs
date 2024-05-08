using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerClass
{
    //Natalie's HealthBar
    public HealthBar healthBar;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Explosion")
            health -= 10;
    }

    private void Die()
    {
        Debug.Log("player died!");
        //Destroy(gameObject);
    }
}
