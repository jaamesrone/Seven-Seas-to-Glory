using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    // Constants
    public float maxForwardSpeed = 10;
    public float forwardSpeedInertia = 0.5f;


    public float minYawPerSecond = 15.0f;
    public float maxYawPerSecond = 5.0f;

    // Current
    public float currentForwardSpeed = 0;
    public float currentYawPerSecond = 0;

    public float inputForwardPercent = 0; // -1 to 1
    public float inputYawPercent = 0;     // -1 to 1

    // External Variables
    public Vector2 seaSpeed = Vector2.zero;

    // Resultant Variables
    private Vector3 desiredRotation = Vector3.zero;

    void DoInput()
    {
        inputForwardPercent = Input.GetAxis("Vertical");
        inputYawPercent = Input.GetAxis("Horizontal");
    }


    void UpdateDesiredVelocities()
    {
        currentForwardSpeed = Mathf.MoveTowards(currentForwardSpeed, inputForwardPercent * maxForwardSpeed, forwardSpeedInertia * Time.deltaTime);

        // Yaw Per Second is mapped max at -maxForwardSpeed and maxForward Sppeed. It is minYawPerSecond 0.
        currentYawPerSecond = Mathf.Lerp(minYawPerSecond, maxYawPerSecond, Mathf.Abs(currentForwardSpeed) / maxForwardSpeed);
    }

    // Per Frame
    void FixedUpdate()
    {
        DoInput();
        UpdateDesiredVelocities();

        // Update Rotation
        transform.Rotate(Vector3.up, currentYawPerSecond * inputYawPercent * Time.deltaTime);

        // Update Position
        transform.Translate(Vector3.left * currentForwardSpeed * Time.deltaTime);

        // Ocean pushing along
        //movedPosition += seaSpeed;
        //desiredPosition = desiredPosition + movedPosition.Scale(Time.deltaTime);
    }

    void LateUpdate()
    {
        float t = Time.deltaTime * 2.0f; // By feel, magic number

        // Apply the desired position and rotation
        transform.rotation = Quaternion.Euler(desiredRotation);
    }
}
