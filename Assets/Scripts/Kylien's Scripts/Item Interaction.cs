using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public GameObject interactionText;
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.F;
    public float spinSpeed = 100f;
    public float rotateSpeed = 50f;

    private bool isTextVisible;

    void Update()
    {
        // Rotate the coin
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        // Check if the player is nearby
        Vector3 playerPosition = Camera.main.transform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        bool playerNearby = distanceToPlayer <= interactionDistance;

        // Show/hide interaction text
        if (playerNearby && !isTextVisible)
        {
            interactionText.SetActive(true);
            isTextVisible = true;
        }
        else if (!playerNearby && isTextVisible)
        {
            interactionText.SetActive(false);
            isTextVisible = false;
        }

        // Handle interaction
        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            // Handle coin pickup
            Destroy(gameObject);
            interactionText.SetActive(false);
            isTextVisible = false;
        }

        // Spin the coin
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
