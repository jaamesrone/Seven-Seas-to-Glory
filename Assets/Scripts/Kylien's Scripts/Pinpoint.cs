using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinpoint : MonoBehaviour
{
    public GameObject waypoint;
    public float rotationSpeed = 180f;

    void Update()
    {
        if (waypoint != null)
        {
            // Calculate the direction to the waypoint
            Vector3 directionToWaypoint = waypoint.transform.position - transform.position;

            // Calculate the angle between the camera's forward direction and the direction to the waypoint
            Vector3 directionToCamera = Camera.main.transform.forward;
            directionToCamera.y = 0; // Ignore vertical component
            float angle = Vector3.SignedAngle(directionToCamera, directionToWaypoint, Vector3.up);

            // Rotate the arrow smoothly based on the angle
            float targetZRotation = Mathf.Clamp(angle, -180f, 180f); // Clamp the angle to -180 to 180 range
            Quaternion targetRotation = Quaternion.Euler(0, 0, -targetZRotation); // Negative rotation for left, positive for right

            // Smoothly rotate the arrow towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
