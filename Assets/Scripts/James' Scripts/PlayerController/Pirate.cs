using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : EnemyClass
{
    private Transform player;
    private Rigidbody rb;

    Animator pirateAnimation;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckPlayerRadius();
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void MoveTowardsPlayer()
    {
        pirateAnimation.SetBool("isRunning", true);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void CheckPlayerRadius()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            LookAtPlayer();
            MoveTowardsPlayer();
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        //attacking
        StartCoroutine(attacking());
    }
    IEnumerator attacking()
    {
        //animation
        Debug.Log("Attacking player!");

        // wait until attack again
        yield return new WaitForSeconds(1f);

        
    }
    public void TakeDamage(float damage)
    {
        // pirate takes dmg
        health -= damage;

        // Check if the pirate's health is below or equal to zero
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Pirate died!");
        Destroy(gameObject);
    }
}
