using System.Collections;
using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    private enum State { Patrolling, AvoidingObstacle, Attacking }
    private State currentState = State.Patrolling;

    public float speed;
    public float rotationSpeed;
    public float detectionDistance; // how far ship checks for obstacles
    public float shootingRadius; // radius to detect player ship

    public float shootingCooldown = 2f; 
    public bool isInHandToHandCombat = false;

    private float turnDuration = 7f;
    private float turnTimer;
    private float shootingTimer;

    public LayerMask obstacleLayer; // this is for unity to know whats an obstacle to avoid for the pirate ship
    private GameObject playerShip;
    public GameObject cannonballPrefab; 
    public Transform[] leftCannonSpawnPoints; 
    public Transform[] rightCannonSpawnPoints; 

    void Start()
    {
        playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
    }

    void Update()
    {
        switch (currentState)//AI state's
        {
            case State.Patrolling:
                Patrolling();
                break;
            case State.AvoidingObstacle:
                AvoidingObstacle();
                break;
            case State.Attacking:
                Attacking();
                break;
        }
    }

    void Patrolling()
    {
        // check for obstacles ahead
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance, obstacleLayer))
        {
            currentState = State.AvoidingObstacle;
            turnTimer = 0; // reset timer for turning
            return;
        }

        // check for player ship within the shooting radius
        if (Vector3.Distance(transform.position, playerShip.transform.position) <= shootingRadius)
        {
            currentState = State.Attacking;
            return;
        }

        // continue moving forward
        MoveForward();

        // randomly decide to turn
        if (turnTimer <= 0)
        {
            DecideTurn();
        }
        else
        {
            turnTimer -= Time.deltaTime;
        }
    }

    void AvoidingObstacle()
    {
        // rotate away from the obstacle
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // continue moving forward
        MoveForward();

        // turn timer
        turnTimer += Time.deltaTime;

        // check if it's time to resume patrolling
        if (turnTimer >= turnDuration || IsPathClear())
        {
            currentState = State.Patrolling;
            turnTimer = 0;
        }
    }

    // the attacking behavior of the ship
   public void Attacking()
    {
        if (isInHandToHandCombat)
        {
            // stop attacking if in handtohand combat
            return;
        }
        // shooting timer 
        shootingTimer += Time.deltaTime;
        // if the shooting timer has passed the cooldown period
        if (shootingTimer >= shootingCooldown)
        {
            // which side of the ship the player is on and shoot from that side
            if (IsPlayerOnLeftSide())
            {
                // if the player is on the left side, shoot cannonballs from left side
                ShootCannonballs(leftCannonSpawnPoints);
            }
            else
            {
                // if the player is not on the left side, shoot from right side
                ShootCannonballs(rightCannonSpawnPoints);
            }
            // resets shooting timer
            shootingTimer = 0f;
        }

        // check to see if the player ship is outside the shooting radius
        if (Vector3.Distance(transform.position, playerShip.transform.position) > shootingRadius)
        {
            // if the player is out of range, patrolling state is now activated >:D
            currentState = State.Patrolling;
        }
    }

    bool IsPlayerOnLeftSide()
    {
        // vec3 for the direction vector from pirate ship to the player ship
        Vector3 toPlayer = playerShip.transform.position - transform.position;
        // a right vec3 towards the player from the ship's forward direction
        Vector3 toPlayerRight = Vector3.Cross(transform.forward, toPlayer);
        // return true if the vec3dot is less than 0.. just means the player is on the left side
        return Vector3.Dot(transform.up, toPlayerRight) < 0;
    }

    // shoots cannonballs from specified spawn points
    void ShootCannonballs(Transform[] cannonSpawnPoints)
    {
        // float for the speed
        float cannonballSpeed = 100f;
        // goes through each spawn point
        foreach (Transform cannonSpawnPoint in cannonSpawnPoints)
        {
            // spawns a cannonball
            GameObject cannonball = Instantiate(cannonballPrefab, cannonSpawnPoint.position, cannonSpawnPoint.rotation);
            // gets the rb
            Rigidbody cannonballRb = cannonball.GetComponent<Rigidbody>();
            // adds velocity for the shooting
            cannonballRb.velocity = cannonSpawnPoint.forward * cannonballSpeed;
        }
    }

    //freezing cannon affecting the reload speed
    public void slowReload(float decreaseValue, float decreaseDuration)
    {
        StartCoroutine(ReduceShotSpeed(decreaseValue, decreaseDuration));
    }
    IEnumerator ReduceShotSpeed(float value, float duration)
    {
        float originalSpeed = shootingCooldown;
        shootingCooldown += value;
        yield return new WaitForSecondsRealtime(duration);
        shootingCooldown = originalSpeed;
    }

    bool IsPathClear()
    {
        // a raycast to check if the path ahead is clear
        return !Physics.Raycast(transform.position, transform.forward, detectionDistance, obstacleLayer);
    }

    void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void DecideTurn()
    {
        // randomly choose a direction and duration for turning
        if (Random.value <= 0.5f)
        {
            rotationSpeed = -Mathf.Abs(rotationSpeed); // turn left
        }
        else
        {
            rotationSpeed = Mathf.Abs(rotationSpeed); // turn right
        }

        turnTimer = Random.Range(2f, turnDuration); // randomize turn duration for more dynamic behavior
    }
    public void SetHandToHandCombat(bool isInCombat)
    {
        isInHandToHandCombat = isInCombat;
    }

}
