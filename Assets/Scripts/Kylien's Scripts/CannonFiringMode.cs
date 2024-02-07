using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonFiringMode : MonoBehaviour
{
    public GameObject cannonballPrefab;
    public Transform cannonballSpawnPoint;
    public float cannonballForce = 100f;
    public float interactionRadius = 5f; // Radius within which the player can interact with the cannon

    private bool isInFiringMode = false;
    private bool hasFired = false;
    private Transform player; // Reference to the player's transform

    private void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Check if player is within interaction radius and press F to toggle firing mode
        if (Vector3.Distance(transform.position, player.position) <= interactionRadius && Input.GetKeyDown(KeyCode.F) && !hasFired)
        {
            Debug.Log("Player entered interaction radius. Press 'F' to enter firing mode.");
            ToggleFiringMode();
        }

        if (isInFiringMode && !hasFired && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Firing cannonball...");
            FireCannonball();
        }

        // Additional key to exit firing mode
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Exiting firing mode.");
            ExitFiringMode();
        }
    }

    private void ToggleFiringMode()
    {
        isInFiringMode = !isInFiringMode;

        // Enable/disable other scripts or components here
        // For demonstration purposes, let's disable a sample component
        // Replace "SampleComponent" with the actual component you want to disable
        PlayerController sampleComponent = GetComponent<PlayerController>();
        if (sampleComponent != null)
        {
            sampleComponent.enabled = !isInFiringMode; // Toggling enabled/disabled state
        }

        if (isInFiringMode)
        {
            Debug.Log("Entered firing mode.");
        }
        else
        {
            Debug.Log("Exited firing mode.");
        }
    }

    private void FireCannonball()
    {
        GameObject cannonball = Instantiate(cannonballPrefab, cannonballSpawnPoint.position, cannonballSpawnPoint.rotation);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(cannonballSpawnPoint.forward * cannonballForce, ForceMode.Impulse);
        }
        hasFired = true;
    }

    private void ExitFiringMode()
    {
        if (isInFiringMode)
        {
            ToggleFiringMode();
        }
    }
}
