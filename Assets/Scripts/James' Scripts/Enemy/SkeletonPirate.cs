using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class SkeletonPirate : EnemyClass
{
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator pirateAnimation;
    [SerializeField] private float blockChance = 0.3f; // Lower chance to block attacks, more agile
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackDelay = 1.0f; // Faster attack speed
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshPro damageTextPrefab;
    public bool isAttacking = false;
    private bool isAwake = false;

    void Start()
    {
        base.health = 100f; // Lower health, emphasizing agility over durability
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        pirateAnimation = GetComponent<Animator>();
        healthBar.SetMaxHealth(health);
        pirateAnimation.Play("Sleeping");

    }

    void Update()
    {
        if (!isAwake)
        {
            CheckAwakening();
        }
        else
        {
            CheckPlayerRadius();
            UpdateAnimation();
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    void CheckAwakening()
    {
        if (Vector3.Distance(player.position, transform.position) < detectionRadius)
        {
            isAwake = true;
            pirateAnimation.StopPlayback(); // Stop the sleeping animation
            pirateAnimation.Play("Idle"); // Transition to awake state, play any transition animation if you have
        }
    }

    void CheckPlayerRadius()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRadius)
        {
            LookAtPlayer();
            agent.destination = player.position;
            agent.isStopped = distanceToPlayer <= attackRange;

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                isAttacking = true;
                AttackPlayer();
            }
            else if (distanceToPlayer > attackRange && isAttacking)
            {
                isAttacking = false;
                pirateAnimation.SetBool("pirateAttack", false);
            }
        }
        else if (isAttacking)
        {
            isAttacking = false;
            pirateAnimation.SetBool("pirateAttack", false);
            agent.isStopped = true;
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
        while (player && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            pirateAnimation.SetBool("pirateAttack", true);
            yield return new WaitForSeconds(attackDelay);
        }
        isAttacking = false;
        pirateAnimation.SetBool("pirateAttack", false);
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

}
