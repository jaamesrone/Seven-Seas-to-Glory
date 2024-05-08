using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    public HealthBar healthBar;
    public float health = 100f;

    private bool hit = false;
    private int addedDamage = 0;

    public KillGain killGain;

    void Start()
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
            killGain.ShipSink();
            Die();
        }
    }

    public void TakeIncrementalDamage(float damage, float time, float increment)
    {
        addedDamage += 1;
        if (!hit)
        {
            hit = true;
            StartCoroutine(IncrementalDamageRoutine(damage, time, increment));
        }
    }

    IEnumerator IncrementalDamageRoutine(float damage, float time, float increment)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < time)
        {
            yield return new WaitForSecondsRealtime(increment);
            TakeDamage(damage * addedDamage);
        }
        addedDamage = 0;
        hit = false;
    }

    private void Die()
    {
        Debug.Log("Enemy Sank!");
        Destroy(gameObject);
    }
}
