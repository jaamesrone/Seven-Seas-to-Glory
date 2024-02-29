using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public LayerMask obstacleLayer; // this is for unity to know whats an obstacle for the ship
    public float detectionDistance; // how far ship checks for obstacles
    public float shootingRadius; // shooting radius to detect player ship
    public GameObject cannonballPrefab; // prefab of the cannonball
    public Transform[] cannonSpawnPoints; // array of empty gameobjects where cannonballs shoot from
    public float shootingCooldown = 2f; // cooldown between shots

    private enum State { Patrolling, AvoidingObstacle, Attacking }
    private State currentState = State.Patrolling;

    private float turnDuration = 7f;
    private float turnTimer;
    private float shootingTimer;
    private GameObject playerShip;

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

    void Attacking()
    {
        // shoot cannonballs at the player ship with cooldown
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingCooldown)
        {
            ShootCannonballs();
            shootingTimer = 0f; // reset shooting timer
        }

        // check if the player ship is out of the shooting radius
        if (Vector3.Distance(transform.position, playerShip.transform.position) > shootingRadius)
        {
            currentState = State.Patrolling;
        }
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

    void ShootCannonballs()
    {
        float cannonballSpeed = 100f;
        // shoot cannonballs from cannonSpawnPoint
        foreach (Transform cannonSpawnPoint in cannonSpawnPoints)
        {
            GameObject cannonball = Instantiate(cannonballPrefab, cannonSpawnPoint.position, cannonSpawnPoint.rotation);
            Rigidbody cannonballRb = cannonball.GetComponent<Rigidbody>();
            cannonballRb.velocity = cannonSpawnPoint.forward * cannonballSpeed; 
        }
    }
}
