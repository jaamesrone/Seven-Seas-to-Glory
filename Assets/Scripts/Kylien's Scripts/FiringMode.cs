using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringMode : MonoBehaviour
{
    public float rotationSpeed = 1f; // Adjust the rotation speed
    public float maxVerticalAngle = 80f; // Maximum vertical angle of the cannon
    public float reloadTime = 3f; // Time to wait before reloading (in seconds)
    public GameObject cannonballPrefab;

    private bool canFire = true;

    void Update()
    {
        // Rotate the cannon based on keyboard input
        float horizontalInput = Input.GetAxis("Horizontal") * rotationSpeed;
        float verticalInput = Input.GetAxis("Vertical") * rotationSpeed;

        // Rotate horizontally around the Y axis
        transform.Rotate(Vector3.up, horizontalInput);

        // Rotate vertically around the local X axis (up and down)
        Vector3 currentRotation = transform.localEulerAngles;
        float newRotationX = currentRotation.x - verticalInput;
        if (newRotationX > 180)
        {
            newRotationX -= 360;
        }
        newRotationX = Mathf.Clamp(newRotationX, -maxVerticalAngle, maxVerticalAngle);
        transform.localEulerAngles = new Vector3(newRotationX, currentRotation.y, currentRotation.z);

        // Fire cannonball from camera position and direction
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            FireCannonball();
            StartCoroutine(ReloadCannon());
        }
    }

    void FireCannonball()
    {
        // Calculate the firing position and direction
        Vector3 firingPosition = transform.position + transform.forward * 1.5f; // Adjust the distance from the cannon
        Vector3 firingDirection = transform.forward;

        // Instantiate and fire the cannonball
        GameObject cannonball = Instantiate(cannonballPrefab, firingPosition, Quaternion.identity);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.velocity = firingDirection * 20f; // Adjust the speed of the cannonball

        // Disable firing until reload is complete
        canFire = false;
    }

    IEnumerator ReloadCannon()
    {
        yield return new WaitForSeconds(reloadTime);
        canFire = true; // Enable firing after reload time
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destroy the enemy
            Destroy(gameObject); // Destroy the cannonball
        }
    }
}
