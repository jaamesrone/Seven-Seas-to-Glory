using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject firing;
    public GameObject normalCannonball;
    public GameObject explodeCannonball;
    public GameObject freezeCannonball;
    public GameObject reticle;
    public GameObject reload;
    public GameObject cannon;

    public float power;
    public float reloadTime = 3f;

    public InventoryUI inventoryActive;
    public Player player;

    public float maxHeight = 10;
    public float minHeight = 1;
    public float increment = 1;

    private GameObject currentCannonball;
    private bool canFire = true;
    private Coroutine reloadCoroutine;

    private void Start()
    {
        reload.SetActive(false);
        currentCannonball = normalCannonball;
    }

    private void Update()
    {
        HandleCannonballSwitching();
        AimCannon();
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            canFire = false;
            FireCannonBall();
        }
    }

    void HandleCannonballSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCannonball(normalCannonball, 2);
        if (Input.GetKeyDown(KeyCode.Alpha4) && player.numExplodeCannonballs > 0) SwitchCannonball(explodeCannonball, 3);
        if (Input.GetKeyDown(KeyCode.Alpha5) && player.numFreezingCannonballs > 0) SwitchCannonball(freezeCannonball, 4);
    }

    void SwitchCannonball(GameObject prefab, int inventoryIndex)
    {
        currentCannonball = prefab;
        inventoryActive.UpdateActive(inventoryIndex);
    }

    void FireCannonBall()
    {
        GameObject cannonBall = Instantiate(currentCannonball, firing.transform.position, Quaternion.identity,firing.transform);
        cannonBall.transform.rotation = firing.transform.rotation;
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();

        if (rb != null)
        {
             rb.AddForce(cannonBall.transform.TransformDirection(Vector3.forward) * power, ForceMode.Impulse);
            // rb.velocity = cannonBall.transform.TransformDirection(Vector3.forward) * power;
        }

        HandleDecrement();

        if (reloadCoroutine != null) StopCoroutine(reloadCoroutine);
        reloadCoroutine = StartCoroutine(ReloadCannon());
    }

    private void HandleDecrement()
    {
        if(currentCannonball == explodeCannonball)
        {
            player.numExplodeCannonballs -= 1;
            if(player.numExplodeCannonballs <= 0)
            {
                currentCannonball = normalCannonball;
            }
        }
        if(currentCannonball == freezeCannonball)
        {
            player.numFreezingCannonballs -= 1;
            if (player.numFreezingCannonballs <= 0)
            {
                currentCannonball = normalCannonball;
            }
        }
    }

    IEnumerator ReloadCannon()
    {
        reload.SetActive(true);
        float cooldownTimer = reloadTime;
        float rotationSpeed = 360f / reloadTime;
        while (cooldownTimer > 0)
        {
            reload.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            yield return null;
            cooldownTimer -= Time.deltaTime;
        }
        canFire = true;
        reload.SetActive(false);
    }

    void AimCannon()
    {
        //Reversed because cannon is upside down
        if (cannon.transform.localRotation.x > -maxHeight && Input.GetKey(KeyCode.W))
        {
            Debug.Log("cannon: " + cannon.transform.localRotation.x);
            cannon.transform.localEulerAngles += new Vector3(-increment, 0, 0);
        }
        if (cannon.transform.localRotation.x <= minHeight && Input.GetKey(KeyCode.S))
        {
            Debug.Log("cannon: " + cannon.transform.localRotation.x);
            cannon.transform.localEulerAngles += new Vector3(increment, 0, 0);
        }
    }
}
