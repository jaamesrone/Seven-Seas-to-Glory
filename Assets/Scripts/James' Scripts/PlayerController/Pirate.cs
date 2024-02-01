using System.Collections;
using UnityEngine;

public class Pirate : EnemyClass
{
    private Transform player;
    private Rigidbody rb;

    Animator pirateAnimation;
    private float attackRange = 4f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        pirateAnimation = GetComponent<Animator>();
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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void CheckPlayerRadius()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            LookAtPlayer();

            if (distanceToPlayer <= attackRange)
            {
                pirateAnimation.SetBool("isRunning", false);
                // The pirate is within attack range, initiate attack animation
                AttackPlayer();
            }
            else
            {
                pirateAnimation.SetBool("pirateAttack", false);
                // The pirate is outside attack range, move towards the player
                MoveTowardsPlayer();
                // Set the "isRunning" parameter to true for movement animation
                pirateAnimation.SetBool("isRunning", true);
            }
        }
        else
        {
            // Player is out of detection radius, stop running animation
            pirateAnimation.SetBool("isRunning", false);
        }
    }

    void AttackPlayer()
    {
            pirateAnimation.SetBool("pirateAttack", true);
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
