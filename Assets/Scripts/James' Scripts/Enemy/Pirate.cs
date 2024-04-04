using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Pirate : EnemyClass
{
    private Transform player;
    private NavMeshAgent agent;
    private Animator pirateAnimation;
    public float attackRange = 5f;
    [SerializeField]
    private float blockChance = 0.3f; // 30% chance to block
    public bool isAttacking = false;

    // natalie's HealthBar
    public HealthBar healthBar;

    // reference to the TextMeshPro component for displaying damage indicator
    public TextMeshPro damageTextPrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        pirateAnimation = GetComponent<Animator>();

        // set max health for health bar
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

            if (distanceToPlayer <= attackRange)
            {
                agent.isStopped = true;

                // check if the pirate is not already in attack mode
                if (!isAttacking)
                {
                    isAttacking = true;
                    AttackPlayer();
                }
            }
            else
            {
                // if the player is out of attack range, switch bool back to false
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
            // if the player is out of detection radius, switch bool back to false
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

        pirateAnimation.SetBool("isRunning", speed > 0);
    }

    void AttackPlayer()
    {
        pirateAnimation.SetBool("pirateAttack",true);
    }

    public void TakeDamage(float damage)
    {

        // random value to determine if the pirate blocks the player attack.
        if (Random.value < blockChance)
        {
            // pirate blocked the attack
            BlockAttack(); 
            Debug.Log("Attack was blocked!");
            return; 
        }
        // deal damage to the pirate
        health -= damage;

        //More of Natalie's Healthbar
        healthBar.SetHealth(health);

        // check if the pirate's health is below or equal to zero
        if (health <= 0f)
        {
            Die();
        }
        else
        {
            // display damage indicator
            DisplayDamageIndicator(transform.position, damage);
        }
    }

    private void DisplayDamageIndicator(Vector3 position, float damage)
    {
        // offset the position to be above the pirate's head
        Vector3 aboveHeadPosition = position + Vector3.up * 0.7f;

        // instantiate the damage text prefab above the head.
        TextMeshPro damageText = Instantiate(damageTextPrefab, aboveHeadPosition, Quaternion.identity);

        // move damage numbers the direction to the camera
        Vector3 toCamera = Camera.main.transform.position - damageText.transform.position;

        // make the damage text face the camera
        damageText.transform.rotation = Quaternion.LookRotation(toCamera);

        // set the damage text
        damageText.text = damage.ToString();

        // start the floating coroutine
        StartCoroutine(FloatDamageText(damageText));

        // destroy the text after a certain time
        Destroy(damageText.gameObject, 1.0f); // change that float number for longer time.
    }


    private IEnumerator FloatDamageText(TextMeshPro damageText)
    {
        float elapsedTime = 0f;
        float floatDuration = 1.0f; // adjust this value to control the duration of floating

        Vector3 startPosition = damageText.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 2.0f; // adjust the float height

        while (elapsedTime < floatDuration)
        {
            // calculate the new position based on interpolation
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / floatDuration);

            // update the text's position
            damageText.transform.position = newPosition;

            // increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // making sure text is at the final position
        damageText.transform.position = endPosition;
    }


    private void Die()
    {
        Debug.Log("Pirate died!");
        Destroy(gameObject);
    }

    void BlockAttack()
    {
        // pirate blocks
        pirateAnimation.SetTrigger("Block");

        // resets trigger after animation
        StartCoroutine(ResetBlockTriggerAfterAnimation());
    }

    IEnumerator ResetBlockTriggerAfterAnimation()
    {
        // resets the trigger after 1 second.
        yield return new WaitForSeconds(1.0f); 

        // reset the trigger "Block"
        pirateAnimation.ResetTrigger("Block");
    }
}
