using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    public float speed = 5f; // The speed at which the enemy ship moves
    public float rotationSpeed = 2f; // The speed at which the enemy ship rotates

    private bool isTurningLeft = false;
    private bool isTurningRight = false;
    private float turnDuration = 2f;
    private float turnTimer = 0f;

    void Update()
    {
        // Calculate the new position of the ship based on its forward direction and speed
        Vector3 newPosition = transform.position + transform.forward * speed * Time.deltaTime;

        // Update turn timer
        turnTimer += Time.deltaTime;

        // If no turn is in progress, decide whether to turn left or right
        if (!isTurningLeft && !isTurningRight)
        {
            DecideTurn();
        }

        // If turning left, rotate left
        if (isTurningLeft)
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);

            // Check if turn duration is reached
            if (turnTimer >= turnDuration)
            {
                isTurningLeft = false;
                turnTimer = 0f;
            }
        }
        // If turning right, rotate right
        else if (isTurningRight)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Check if turn duration is reached
            if (turnTimer >= turnDuration)
            {
                isTurningRight = false;
                turnTimer = 0f;
            }
        }

        // Apply the new position
        transform.position = newPosition;
    }

    // decision about turning left or right
    void DecideTurn()
    {
        // there's a 30% chance of turning left and a 70% chance of turning right
        float randomValue = Random.value;
        if (randomValue <= 0.3f)
        {
            isTurningLeft = true;
        }
        else
        {
            isTurningRight = true;
        }
    }
}
