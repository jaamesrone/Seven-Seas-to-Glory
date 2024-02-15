using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FiringMode : MonoBehaviour
{
    public float reloadTime = 3f;
    public GameObject cannonballPrefab;
    public Text cooldownText; 

    private bool canFire = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            FireCannonball();
            StartCoroutine(ReloadCannon());
        }
    }

    void FireCannonball() //Mechanic to shoot cannon ball
    {
        Vector3 firingPosition = transform.position + transform.forward * 1.5f;
        Vector3 firingDirection = transform.forward;

        GameObject cannonball = Instantiate(cannonballPrefab, firingPosition, Quaternion.identity);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.velocity = firingDirection * 20f;

        canFire = false;
    }

    IEnumerator ReloadCannon() //Mechanic to Reload
    {
        float cooldownTimer = reloadTime;
        while (cooldownTimer > 0)
        {
            cooldownText.text = "Cooldown: " + Mathf.Ceil(cooldownTimer).ToString(); // Update cooldown text
            yield return new WaitForSeconds(1f);
            cooldownTimer -= 1f;
        }
        cooldownText.text = ""; // Clear cooldown text when reloading is finished
        canFire = true;
    }

    void OnCollisionEnter(Collision collision) //Currently Experimenting
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
