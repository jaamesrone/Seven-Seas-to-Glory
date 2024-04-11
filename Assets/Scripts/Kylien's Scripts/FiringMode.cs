using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Removed unnecessary using directives for cleaner code

public class FiringMode : MonoBehaviour
{
    public RectTransform reticleRectTransform;
    public float reloadTime = 3f;
    public GameObject normalCannonballPrefab;
    public GameObject explodingCannonballPrefab;
    public GameObject freezingCannonballPrefab;
    public GameObject firingPoint;
    public TextMeshProUGUI cooldownText;
    public GameObject reloadReticle;

    private bool canFire = true;
    private GameObject currentCannonball;
    private Coroutine reloadCoroutine;

    public InventoryUI inventoryActive;
    public Player player;

    // Variables for aiming functionality
    public float maxHeight = 10;
    public float increment = 1;
    public float aim = 1;

    void Start()
    {
        reloadReticle.SetActive(false); // Start with reload reticle disabled
        currentCannonball = normalCannonballPrefab; // Default to normal cannonball
    }

    void Update()
    {
        HandleCannonballSwitching();
        HandleAiming();
        HandleFiring();
    }

    void HandleCannonballSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCannonball(normalCannonballPrefab, 2);
        if (Input.GetKeyDown(KeyCode.Alpha4) && player.numExplodeCannonballs > 0) SwitchCannonball(explodingCannonballPrefab, 3);
        if (Input.GetKeyDown(KeyCode.Alpha5) && player.numFreezingCannonballs > 0) SwitchCannonball(freezingCannonballPrefab, 4);
    }

    void SwitchCannonball(GameObject prefab, int inventoryIndex)
    {
        currentCannonball = prefab;
        inventoryActive.UpdateActive(inventoryIndex);
    }

    void HandleAiming()
    {
        aim = Mathf.Clamp(aim + increment * (Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0), 1, maxHeight);
        UpdateReticlePosition();
    }

    void UpdateReticlePosition()
    {
        float screenRange = Screen.height / 50;
        float newYPosition = Mathf.Lerp(0, screenRange, (aim - 1) / (maxHeight - 1));
        Vector3 newPosition = reticleRectTransform.anchoredPosition;
        newPosition.y = newYPosition;
        reticleRectTransform.anchoredPosition = newPosition;
    }

    void HandleFiring()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            FireCannonball();
            if (reloadCoroutine != null) StopCoroutine(reloadCoroutine);
            reloadCoroutine = StartCoroutine(ReloadCannon());
        }
    }

    void FireCannonball()
    {
        Vector3 firingPosition = transform.position + transform.forward * 1.5f;
        GameObject cannonball = Instantiate(currentCannonball, firingPosition, Quaternion.identity);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * 20f * aim; // Adjust velocity based on aim

        canFire = false;
    }

    IEnumerator ReloadCannon()
    {
        reloadReticle.SetActive(true);
        float cooldownTimer = reloadTime;
        float rotationSpeed = 360f / reloadTime;
        while (cooldownTimer > 0)
        {
            cooldownText.text = $"Cooldown: {Mathf.Ceil(cooldownTimer)}";
            reloadReticle.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            yield return null;
            cooldownTimer -= Time.deltaTime;
        }
        cooldownText.text = "";
        canFire = true;
        reloadReticle.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
