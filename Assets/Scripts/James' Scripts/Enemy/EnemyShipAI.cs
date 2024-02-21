using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    public float speed = 5f; // The speed at which the enemy ship moves
    public float rotationSpeed = 2f; // The speed at which the enemy ship rotates

    private bool isTurningLeft = false;
    private bool isTurningRight = false;
    private float turnDuration = 7f;
    private float turnTimer = 0f;

    void Update()
    {
        ShipAIMovement(); //ships ai movement
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

    void ShipAIMovement()
    {
        // vector 3 new position of the ship is based on its forward direction and speed
        Vector3 newPosition = transform.position + transform.forward * speed * Time.deltaTime;

        // update turn timer
        turnTimer += Time.deltaTime;

        // if no turns is in progress, decide whether to turn left or right
        if (!isTurningLeft && !isTurningRight)
        {
            DecideTurn();
        }

        // turning left, rotate left
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
        // turning right, rotate right
        else if (isTurningRight)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // check if turn duration is reached
            if (turnTimer >= turnDuration)
            {
                isTurningRight = false;
                turnTimer = 0f;
            }
        }

        // Apply the new position
        transform.position = newPosition;
    }
}
