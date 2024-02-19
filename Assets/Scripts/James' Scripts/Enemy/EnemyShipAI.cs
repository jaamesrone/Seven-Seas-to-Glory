using UnityEngine;

public class EnemyShipAI : MonoBehaviour
{
    public float speed = 5f; // The speed at which the enemy ship moves

    void Update()
    {
        // Calculate the new position of the ship based on its right direction and speed
        Vector3 newPosition = transform.position + transform.forward * speed * Time.deltaTime;

        // Rotate the ship to face the new position
    //    transform.LookAt();

        // Move the ship to the new position
        transform.position = newPosition;
    }
}
