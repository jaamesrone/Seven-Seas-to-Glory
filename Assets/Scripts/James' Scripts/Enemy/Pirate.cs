using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Pirate : EnemyClass
{
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator pirateAnimation;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private float blockChance = 0.3f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshPro damageTextPrefab;
    public bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        pirateAnimation = GetComponent<Animator>();
        healthBar.SetMaxHealth(health);
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
