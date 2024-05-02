using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Friend : EnemyClass
{
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator pirateAnimation;
    [SerializeField] private float blockChance = 0.7f; // higher chance to block attacks
    [SerializeField] private float attackRange = 5f; // higher chance to block attacks
    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float followDistance = 2f; // distance to maintain from the player

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshPro damageTextPrefab;
    public bool isAttacking = false;
    private Transform targetEnemy;
    private Coroutine attackRoutine = null;

    void Start()
    {
        base.health = 150f;
        agent = GetComponent<NavMeshAgent>();
        pirateAnimation = GetComponent<Animator>();
        healthBar.SetMaxHealth(health);
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        Transform closestEnemy = FindClosestEnemy();
        if (closestEnemy != null && Vector3.Distance(transform.position, closestEnemy.position) <= maxDistance)
        {
            targetEnemy = closestEnemy;
            CheckTargetDistanceAndEngage(targetEnemy);
        }
        else
        {
            if (isAttacking)
            {
                StopAttacking();
            }
            targetEnemy = null;
            FollowPlayer();
        }
        UpdateAnimation();
    }
    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistanceSqr = maxDistance * maxDistance;  // Use squared distance for performance
        Transform closestEnemy = null;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Vector3 directionToTarget = enemy.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    void CheckTargetDistanceAndEngage(Transform target)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= maxDistance)
        {
            agent.destination = target.position;
            LookAtTarget(target);
            if (distanceToTarget <= attackRange)
            {
                EngageTarget();
            }
            else
            {
                isAttacking = false;
                agent.isStopped = false;
                pirateAnimation.SetBool("pirateAttack", false);
            }
        }
        else
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 stopPosition = player.position - directionToPlayer * followDistance;

            agent.destination = stopPosition;
            LookAtTarget(player);

            agent.stoppingDistance = followDistance;

            isAttacking = false;
            pirateAnimation.SetBool("pirateAttack", false);
        }
    }

    void EngageTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetEnemy.position);
        agent.destination = targetEnemy.position;
        LookAtTarget(targetEnemy);

        if (distanceToTarget <= attackRange)
        {
            if (!isAttacking)
            {
                StartAttacking();
            }
        }
        else if (isAttacking)
        {
            StopAttacking();
        }
    }
    void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            pirateAnimation.SetBool("pirateAttack", false);
            agent.isStopped = false;
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }
        }
    }
    void StartAttacking()
    {
        isAttacking = true;
        pirateAnimation.SetBool("pirateAttack", true);
        agent.isStopped = true;
        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(AttackPlayerRepeatedly());
        }
    }
    void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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
        while (targetEnemy != null && Vector3.Distance(transform.position, targetEnemy.position) <= attackRange)
        {
            pirateAnimation.SetBool("pirateAttack", true);
            yield return new WaitForSeconds(attackDelay);
        }

        StopAttacking();
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
