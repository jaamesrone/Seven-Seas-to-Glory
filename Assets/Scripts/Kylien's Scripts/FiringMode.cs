using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine.UI;

public class FiringMode : MonoBehaviour
{
    public RectTransform reticleRectTransform;


    public float reloadTime = 3f;
    public GameObject normalCannonballPrefab;
    public GameObject explodingCannonballPrefab;
    public GameObject freezingCannonballPrefab;
    public TextMeshProUGUI cooldownText;
    public GameObject reloadReticle;

    private bool canFire = true;

    private GameObject currentCannonball;
    private GameObject activeCannonball;

    public InventoryUI inventoryActive;
    public Player player;

    // Natalie's aiming
    public float maxHeight = 10;
    public float increment = 1;
    public float aim = 1;

    void Start()
    {
        reloadReticle.SetActive(false); // Start with reload reticle disabled
    }

    void Update()
    {
        if (currentCannonball == null)
        {
            currentCannonball = normalCannonballPrefab;
            inventoryActive.UpdateActive(2);
        }
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            FireCannonball();
            StartCoroutine(ReloadCannon());
        }

        // Check if the active cannonball is destroyed and cancel the reload coroutine if it is
        if (activeCannonball == null)
        {
            StopCoroutine(ReloadCannon());
            reloadReticle.SetActive(false);
            cooldownText.text = "";
            canFire = true;
        }

        // Natalie's aiming
        if (Input.GetKey(KeyCode.W) && aim < maxHeight)
        {
            aim += increment;
        }
        if (Input.GetKey(KeyCode.S) && aim > 1)
        {
            aim -= increment;
        }

        UpdateReticlePosition();

        //switching cannonballs
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentCannonball = normalCannonballPrefab;
            inventoryActive.UpdateActive(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentCannonball = explodingCannonballPrefab;
            inventoryActive.UpdateActive(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentCannonball = freezingCannonballPrefab;
            inventoryActive.UpdateActive(4);
        }
    }

    void UpdateReticlePosition() //james' scriptS
    {
        
        float screenRange = Screen.height / 50;

        // newYPosition is consistent and clamped
        float newYPosition = Mathf.Lerp(0, screenRange, (aim - 1) / (maxHeight - 1)); // Linearly interpolate position based on aim

        // Update the reticle's position
        Vector3 newPosition = reticleRectTransform.anchoredPosition;
        newPosition.y = newYPosition;
        reticleRectTransform.anchoredPosition = newPosition;
    }

    void FireCannonball()
    {
        Vector3 firingPosition = transform.position + transform.forward * 1.5f;
        Vector3 firingDirection = transform.forward * aim; // Use the player's forward direction

        GameObject cannonball = Instantiate(currentCannonball, firingPosition, Quaternion.identity);
        activeCannonball = cannonball; // Set the active cannonball
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.velocity = firingDirection * 20f;

        if(currentCannonball == explodingCannonballPrefab && player.numExplodeCannonballs <= 0)
        {
            currentCannonball = normalCannonballPrefab;
            inventoryActive.UpdateActive(2);
        }
        if (currentCannonball == freezingCannonballPrefab && player.numFreezingCannonballs <= 0)
        {
            currentCannonball = normalCannonballPrefab;
            inventoryActive.UpdateActive(2);
        }

        canFire = false;
    }

    IEnumerator ReloadCannon()
    {
        reloadReticle.SetActive(true); // Show reload reticle when reloading
        float cooldownTimer = reloadTime;
        float rotationSpeed = 360f / reloadTime; // Calculate rotation speed to complete a full circle in reloadTime seconds
        while (cooldownTimer > 0)
        {
            cooldownText.text = "Cooldown: " + Mathf.Ceil(cooldownTimer).ToString();
            reloadReticle.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // Rotate reload reticle
            yield return null;
            cooldownTimer -= Time.deltaTime;
        }
        cooldownText.text = "";
        canFire = true;
        reloadReticle.SetActive(false); // Hide reload reticle when reloading is finished
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
