using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Pirate : EnemyClass
{
    private Transform player;
    private NavMeshAgent agent;
    private Animator pirateAnimation;
    private float attackRange = 5f;

    public bool isAttacking = false;

    //Natalie's HealthBar
    public HealthBar healthBar;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        pirateAnimation = GetComponent<Animator>();

        //more of Natalie's HealthBar
        healthBar.SetMaxHealth(health);

        // Disable NavMeshAgent on Awake
        agent.enabled = false;
        StartCoroutine(EnableAgentAfterDelay(2f));
    }

    IEnumerator EnableAgentAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Re-enable NavMeshAgent after a couple of seconds
        agent.enabled = true;
    }

    void Update()
    {
        CheckPlayerRadius();
        UpdateAnimation();
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void CheckPlayerRadius()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            LookAtPlayer();

            if (distanceToPlayer <= attackRange)
            {
                agent.isStopped = true;

                // Check if the pirate is not already in attack mode
                if (!isAttacking)
                {
                    isAttacking = true;
                    AttackPlayer();
                }
            }
            else
            {
                // If the player is out of attack range, switch bool back to false
                if (isAttacking)
                {
                    isAttacking = false;
                    pirateAnimation.SetBool("pirateAttack", false);
                }

                agent.isStopped = false;
                agent.destination = player.position;
            }
        }
        else
        {
            // If the player is out of detection radius, switch bool back to false
            if (isAttacking)
            {
                isAttacking = false;
                pirateAnimation.SetBool("pirateAttack", false);
            }

            agent.isStopped = true;
        }
    }

    void UpdateAnimation()
    {
        float speed = agent.velocity.magnitude;

        if (speed > 0)
        {
            pirateAnimation.SetBool("isRunning", true);
        }
        else
        {
            pirateAnimation.SetBool("isRunning", false);
        }
    }

    void AttackPlayer()
    {
        pirateAnimation.SetTrigger("pirateAttack");
    }

    public void TakeDamage(float damage)
    {
        // pirate takes dmg
        health -= damage;

        //even more of Natalie's HealthBar
        healthBar.SetHealth(health);

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
