using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringMode : MonoBehaviour
{
    public float rotationSpeed = 1f; 
    public float maxVerticalAngle = 80f; 
    public float reloadTime = 3f; 
    public GameObject cannonballPrefab;

    private bool canFire = true;

    void Update()
    {
       
        float horizontalInput = Input.GetAxis("Horizontal") * rotationSpeed;
        float verticalInput = Input.GetAxis("Vertical") * rotationSpeed;

        // Rotate horizontally around the Y axis
        transform.Rotate(Vector3.up, horizontalInput);

       
        Vector3 currentRotation = transform.localEulerAngles;
        float newRotationX = currentRotation.x - verticalInput;
        if (newRotationX > 180)
        {
            newRotationX -= 360;
        }
        newRotationX = Mathf.Clamp(newRotationX, -maxVerticalAngle, maxVerticalAngle);
        transform.localEulerAngles = new Vector3(newRotationX, currentRotation.y, currentRotation.z);

       
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            FireCannonball();
            StartCoroutine(ReloadCannon());
        }
    }

    void FireCannonball()
    {
        
        Vector3 firingPosition = transform.position + transform.forward * 1.5f; 
        Vector3 firingDirection = transform.forward;

       
        GameObject cannonball = Instantiate(cannonballPrefab, firingPosition, Quaternion.identity);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.velocity = firingDirection * 20f; 

       
        canFire = false;
    }

    IEnumerator ReloadCannon()
    {
        yield return new WaitForSeconds(reloadTime);
        canFire = true; 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); 
            Destroy(gameObject); 
        }
    }
}
