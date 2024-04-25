using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Friend : EnemyClass
{
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator pirateAnimation;
    [SerializeField] private float blockChance = 0.7f; // Higher chance to block attacks
    [SerializeField] private float attackRange = 5f; // Higher chance to block attacks
    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private float jumpDistance = 10f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshPro damageTextPrefab;
    public bool isAttacking = false;
    public float jumpForce; // The force of the jump
    public bool hasJumpedToEnemy = false;
    private Transform lastTarget;

    void Start()
    {
        base.health = 150f; // Higher health
        Transform closestEnemyTransform = FindClosestEnemy();
        if (closestEnemyTransform != null)
        {
            player = closestEnemyTransform;
        }
        agent = GetComponent<NavMeshAgent>();
        pirateAnimation = GetComponent<Animator>();
        healthBar.SetMaxHealth(health);
    }

    void Update()
    {
        if (player == null || player != lastTarget)
        {
            player = FindClosestEnemy();
            if (player != null)
            {
                lastTarget = player;
                hasJumpedToEnemy = false; // Reset jump status when new enemy is targeted
            }
            else
            {
                return;
            }
        }
        CheckPlayerRadius();
        UpdateAnimation();

    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in enemies)
        {
            float maxDistance = 50f;
            float maxDistanceSqr = maxDistance * maxDistance;

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr && dSqrToTarget <= maxDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy = potentialTarget.transform;
            }
        }

        return closestEnemy;
    }

    private void JumpTowardsEnemy()
    {
        agent.enabled = false;  // Disable the NavMeshAgent before the jump
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 jumpVector = Vector3.up * jumpForce + directionToPlayer * jumpForce * 0.5f;
        rb.AddForce(jumpVector, ForceMode.Impulse);
        pirateAnimation.SetTrigger("Jump");
        StartCoroutine(EnableNavMeshAgentAfterJump());  // Re-enable the agent after a delay
    }

    IEnumerator EnableNavMeshAgentAfterJump()
    {
        yield return new WaitForSeconds(1.5f);  // Wait for the duration of the jump
        agent.enabled = true;
        if (!agent.isOnNavMesh)
        {
            agent.Warp(transform.position);  // Warp to the current position if off the NavMesh
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void CheckPlayerRadius()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < jumpDistance)
        {
            LookAtPlayer();
            agent.destination = player.position;

            // Stop the agent and jump if within jump distance but outside of attack range
            if (distanceToPlayer <= jumpDistance && distanceToPlayer > attackRange)
            {
                if (!hasJumpedToEnemy)
                {
                    agent.isStopped = true;
                    Debug.Log("Attempting to jump towards enemy."); // Debug line
                    JumpTowardsEnemy();
                    hasJumpedToEnemy = true;
                }
            }
            else if (distanceToPlayer <= attackRange)
            {
                agent.isStopped = true;
                if (!isAttacking)
                {
                    isAttacking = true;
                    AttackPlayer();
                }
            }
            else
            {
                agent.isStopped = false;
            }
        }
        else
        {
            isAttacking = false;
            pirateAnimation.SetBool("pirateAttack", false);
            agent.isStopped = false;
        }
    }

    public void TeleportToShip(GameObject destination)
    {
        if (destination != null)
        {
            agent.enabled = false;
            // Define the range of the offset
            float offsetRange = 4.0f; // Adjust the range as needed

            // Generate a random offset within the range
            Vector3 offset = new Vector3(Random.Range(-offsetRange, offsetRange), 0, Random.Range(-offsetRange, offsetRange));

            // Compute the new position with the offset
            Vector3 newPosition = destination.transform.position + offset;

            // Set the transform position and warp the agent
            transform.position = newPosition;
            agent.Warp(newPosition);
            agent.enabled = true;
            hasJumpedToEnemy = false;
        }
    }


    void UpdateAnimation()
    {
        pirateAnimation.SetBool("isRunning", agent.velocity.magnitude > 0);
    }

    void AttackPlayer()
    {
        StartCoroutine(AttackPlayerRepeatedly());
    }

    IEnumerator AttackPlayerRepeatedly()
    {
        while (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            pirateAnimation.SetBool("pirateAttack", true);
            yield return new WaitForSeconds(attackDelay);
        }

        isAttacking = false;
        pirateAnimation.SetBool("pirateAttack", false);
    }



    public void TakeDamage(float damage)
    {
        if (Random.value < blockChance)
        {
            BlockAttack();
            return;
        }
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0f) Die();
        else DisplayDamageIndicator(transform.position, damage);
    }

    void DisplayDamageIndicator(Vector3 position, float damage)
    {
        Vector3 aboveHeadPosition = position + Vector3.up * 0.7f;
        TextMeshPro damageText = Instantiate(damageTextPrefab, aboveHeadPosition, Quaternion.identity);
        Vector3 toCamera = Camera.main.transform.position - damageText.transform.position;
        damageText.transform.rotation = Quaternion.LookRotation(toCamera);
        damageText.text = damage.ToString();
        StartCoroutine(FloatDamageText(damageText));
        Destroy(damageText.gameObject, 1.0f);
    }

    IEnumerator FloatDamageText(TextMeshPro damageText)
    {
        float elapsedTime = 0f;
        float floatDuration = 1.0f;
        Vector3 startPosition = damageText.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 2.0f;

        while (elapsedTime < floatDuration)
        {
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / floatDuration);
            damageText.transform.position = newPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        damageText.transform.position = endPosition;
    }

    void Die()
    {
        Debug.Log("Pirate died!");
        Destroy(gameObject);
    }

    void BlockAttack()
    {
        pirateAnimation.SetTrigger("Block");
        StartCoroutine(ResetBlockTriggerAfterAnimation());
    }

    IEnumerator ResetBlockTriggerAfterAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        pirateAnimation.ResetTrigger("Block");
    }


}
