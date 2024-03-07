using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine.UI;

public class FiringModue : MonoBehaviour
{
    public float reloadTime = 3f;
    public GameObject cannonballPrefab;
    public TextMeshProUGUI cooldownText;
    public GameObject reloadReticle;

    private bool canFire = true;

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
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            FireCannonball();
            StartCoroutine(ReloadCannon());
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
    }

    void FireCannonball()
    {
        Vector3 firingPosition = transform.position + transform.forward * 1.5f;
        Vector3 firingDirection = transform.TransformDirection(Vector3.forward) * aim;

        GameObject cannonball = Instantiate(cannonballPrefab, firingPosition, Quaternion.identity);
        Rigidbody2D rb = cannonball.GetComponent<Rigidbody2D>();
        rb.velocity = firingDirection * 20f;

        canFire = false;
    }

    IEnumerator ReloadCannon()
    {
        reloadReticle.SetActive(true); // Show reload reticle when reloading
        float cooldownTimer = reloadTime;
        while (cooldownTimer > 0)
        {
            cooldownText.text = "Cooldown: " + Mathf.Ceil(cooldownTimer).ToString();
            reloadReticle.transform.Rotate(Vector3.forward * Time.deltaTime * 100f); // Rotate reload reticle
            yield return new WaitForSeconds(1f);
            cooldownTimer -= 1f;
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
