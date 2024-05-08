using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class ImperialPirate : EnemyClass
{
    private bool awaitingRecruitmentDecision = false; // Add this line at the beginning of the class
    [SerializeField] private Transform player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator pirateAnimation;
    [SerializeField] private float blockChance = 0.7f; // Higher chance to block attacks
    [SerializeField] private float attackRange = 5f; // Higher chance to block attacks
    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshPro damageTextPrefab;
    //[SerializeField] private GameObject backupPiratePrefab; // Prefab for calling backup
    public bool isAttacking = false;
    [SerializeField] private GameObject friendlyPiratePrefab; 
    [SerializeField] private TMP_Text recruitmentText; 

    void Start()
    {
        base.health = 150f; // Higher health
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        pirateAnimation = GetComponent<Animator>();
        healthBar.SetMaxHealth(health);

        recruitmentText = GameObject.Find("recruitment").GetComponent<TextMeshProUGUI>();
        if (recruitmentText != null)
        {
            recruitmentText.text = "";
        }
        else
        {
            Debug.LogError("Recruitment text UI element not found!");
        }
    }

    void Update()
    {
        CheckPlayerRadius();
        UpdateAnimation();
        if (awaitingRecruitmentDecision)
        {
            Recruitment();
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
        recruitmentText.text = "\tDo you want to recruit this pirate? J/M.\n\tteleport pirates to you 'T'";
        awaitingRecruitmentDecision = true; // The pirate is now waiting for a decision
        agent.isStopped = true; 
    }
    void Recruitment()
    {
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                awaitingRecruitmentDecision = false;
                recruitmentText.text = "";
                Instantiate(friendlyPiratePrefab, transform.position, Quaternion.identity); // Instantiate a friendly pirate
                Destroy(gameObject); // Destroy the current object
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                awaitingRecruitmentDecision = false;
                recruitmentText.text = "";
                Debug.Log("Pirate died!");
                Destroy(this.gameObject);
            }
        }
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
