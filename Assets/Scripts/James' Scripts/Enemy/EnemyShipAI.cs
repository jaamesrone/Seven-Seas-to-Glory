using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 2f;
    public LayerMask obstacleLayer; // Define which layers are considered obstacles
    public float detectionDistance = 10f; // How far ahead the ship checks for obstacles

    private enum State { Patrolling, AvoidingObstacle }
    private State currentState = State.Patrolling;

    private float turnDuration = 7f;
    private float turnTimer;

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrolling();
                break;
            case State.AvoidingObstacle:
                AvoidingObstacle();
                break;
        }
    }

    void Patrolling()
    {
        // Check for obstacles ahead
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance, obstacleLayer))
        {
            currentState = State.AvoidingObstacle;
            turnTimer = 0; // Reset timer for turning
            return; // Skip the rest of the patrolling logic this frame
        }

        // Continue moving forward
        MoveForward();

        // Randomly decide to turn
        if (turnTimer <= 0)
        {
            DecideTurn(); // Decide whether and when to turn
        }
        else
        {
            turnTimer -= Time.deltaTime;
        }
    }

    void AvoidingObstacle()
    {
        // Rotate away from the obstacle
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Continue moving forward
        MoveForward();

        // Increment turn timer
        turnTimer += Time.deltaTime;

        // Check if it's time to resume patrolling
        if (turnTimer >= turnDuration || IsPathClear())
        {
            currentState = State.Patrolling;
            turnTimer = 0; // Reset turn timer
        }
    }

    bool IsPathClear()
    {
        // Perform a raycast to check if the path ahead is clear
        return !Physics.Raycast(transform.position, transform.forward, detectionDistance, obstacleLayer);
    }

    void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void DecideTurn()
    {
        // Randomly choose a direction and duration for turning
        if (Random.value <= 0.5f)
        {
            rotationSpeed = -Mathf.Abs(rotationSpeed); // Turn left
        }
        else
        {
            rotationSpeed = Mathf.Abs(rotationSpeed); // Turn right
        }

        turnTimer = Random.Range(2f, turnDuration); // Randomize turn duration for more dynamic behavior
    }
}
