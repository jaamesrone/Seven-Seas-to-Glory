using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerClass
{
    public HealthBar healthBar;
    private bool isHealing = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            health = 50f;
            healthBar.SetHealth(health);
        }

        if (health < 100f && !isHealing)
        {
            StartCoroutine(HealOverTime());
        }
    }

    private IEnumerator HealOverTime()
    {
        isHealing = true;
        while (health < 100f)
        {
            health += 5f;
            healthBar.SetHealth(health);
            yield return new WaitForSeconds(1f);
        }
        isHealing = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        //Destroy(gameObject);
    }
}