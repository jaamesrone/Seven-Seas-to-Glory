using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class SharkPirate : EnemyClass
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator sharkAnimation;
    public float blockChance = 0.3f; // 30% chance to block
    public bool isCharging = false;
    public float chargeSpeedMultiplier = 2f;
    public float stealthDetectionMultiplier = 0.5f;
    public float chargeRange = 10f;
    public HealthBar healthBar;
    public TextMeshPro damageTextPrefab;

    // Constructor to initialize the SharkPirate
    public SharkPirate(Transform player, NavMeshAgent agent, Animator sharkAnimation, HealthBar healthBar, TextMeshPro damageTextPrefab)
    {
        this.player = player;
        this.agent = agent;
        this.sharkAnimation = sharkAnimation;
        this.healthBar = healthBar;
        this.damageTextPrefab = damageTextPrefab;

        healthBar.SetMaxHealth(health);
    }

    // Function to be called from MonoBehaviour's Update
    public void OnUpdate()
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
        bool inStealthRange = distanceToPlayer < detectionRadius * stealthDetectionMultiplier;

        if (!inStealthRange)
        {
            LookAtPlayer();
        }

        if (distanceToPlayer < chargeRange && !isCharging)
        {
            isCharging = true;
            ChargeAttack();
        }
        else if (distanceToPlayer >= chargeRange && isCharging)
        {
            isCharging = false;
            sharkAnimation.SetBool("isCharging", false);
            agent.speed /= chargeSpeedMultiplier; // Reset speed after charge
        }

        if (!isCharging)
        {
            agent.isStopped = distanceToPlayer < detectionRadius;
            agent.destination = player.position;
        }
    }

    void UpdateAnimation()
    {
        float speed = agent.velocity.magnitude;
        sharkAnimation.SetBool("isSwimming", speed > 0);
    }

    void ChargeAttack()
    {
        agent.speed *= chargeSpeedMultiplier; // Increase speed for the charge
        sharkAnimation.SetTrigger("sharkCharge");
        // Logic to move towards the player quickly, simulating a charge attack
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
        sharkAnimation.SetTrigger("Block");

        // resets trigger after animation
        StartCoroutine(ResetBlockTriggerAfterAnimation());
    }

    IEnumerator ResetBlockTriggerAfterAnimation()
    {
        // resets the trigger after 1 second.
        yield return new WaitForSeconds(1.0f);

        // reset the trigger "Block"
        sharkAnimation.ResetTrigger("Block");
    }

    // TakeDamage, Die, DisplayDamageIndicator, and other methods would be similar to the ImperialPirate class, adjusted for shark-specific behavior
}
