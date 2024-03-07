using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine.UI;

public class FiringModue : MonoBehaviour
{
    public float reloadTime = 3f;
<<<<<<< Updated upstream
    public GameObject normalCannonballPrefab;
    public GameObject explodingCannonballPrefab;
    public GameObject freezingCannonballPrefab;
    public TextMeshProUGUI cannonballDisplay;
    public TextMeshProUGUI cooldownText; 

    private bool canFire = true;

    private GameObject currentCannonball;

    //Natalie's aiming
=======
    public GameObject cannonballPrefab;
    public TextMeshProUGUI cooldownText;
    public Image clockwiseReticle;
    public Image counterClockwiseReticle;

    private bool canFire = true;

    // Natalie's aiming
>>>>>>> Stashed changes
    public float maxHeight = 10;
    public float increment = 1;
    public float aim = 1;

    void Start()
    {
        // Start with the counter-clockwise reticle disabled
        counterClockwiseReticle.enabled = false;
    }

    void Update()
    {
        //ensures that there's always a cannonball for currentCannonball. will be esspecially useful for special cannonball running out
        if (currentCannonball == null)
        {
            currentCannonball = normalCannonballPrefab;
            cannonballDisplay.text = "Normal Cannonball";
        }

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

<<<<<<< Updated upstream
        //switching cannonballs
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentCannonball = normalCannonballPrefab;
            cannonballDisplay.text = "Normal Cannonball";
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentCannonball = explodingCannonballPrefab;
            cannonballDisplay.text = "Exploding Cannonball";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentCannonball = freezingCannonballPrefab;
            cannonballDisplay.text = "Freezing Cannonball";
        }
=======
        // Rotate the reticles
        clockwiseReticle.transform.Rotate(Vector3.forward * Time.deltaTime * 100f);
        counterClockwiseReticle.transform.Rotate(Vector3.back * Time.deltaTime * 100f);
>>>>>>> Stashed changes
    }

    void FireCannonball()
    {
        Vector3 firingPosition = transform.position + transform.forward * 1.5f;
        Vector3 firingDirection = transform.TransformDirection(Vector3.forward) * aim;

        GameObject cannonball = Instantiate(currentCannonball, firingPosition, Quaternion.identity);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.velocity = firingDirection * 20f;

        canFire = false;
        counterClockwiseReticle.enabled = true;
    }

    IEnumerator ReloadCannon()
    {
        float cooldownTimer = reloadTime;
        while (cooldownTimer > 0)
        {
            cooldownText.text = "Cooldown: " + Mathf.Ceil(cooldownTimer).ToString();
            yield return new WaitForSeconds(1f);
            cooldownTimer -= 1f;
        }
        cooldownText.text = "";
        canFire = true;
        counterClockwiseReticle.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
