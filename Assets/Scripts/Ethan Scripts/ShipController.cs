using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3 desiredPosition = Vector3.zero;
    public Vector3 desiredRotation = Vector3.zero;

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
        desiredRotation.y += currentYawPerSecond * inputYawPercent * Time.deltaTime;

        // Update Position
        Vector3 forwardVector = new Vector3(0, desiredRotation.y, 0);
        Vector3 positionDelta = new Vector3(currentForwardSpeed, 0, 0); //* forwardVector;

        // Ocean pushing along
        //movedPosition += seaSpeed;
        //desiredPosition = desiredPosition + movedPosition.Scale(Time.deltaTime);
    }

    void LateUpdate()
    {
        float t = Time.deltaTime * 2.0f; // By feel, magic number

        // Apply the desired position and rotation
        transform.position = Vector3.Lerp(transform.position, desiredPosition, t);
        transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, desiredRotation, t));
    }
}
