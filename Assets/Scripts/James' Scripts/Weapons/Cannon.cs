using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject[] firing;
    public GameObject normalCannonball;
    public GameObject explodeCannonball;
    public GameObject freezeCannonball;
    public GameObject reticle;
    public GameObject reload;
    public GameObject[] cannons;

    public float power;
    public float reloadTime = 3f;

    public InventoryUI inventoryActive;
    public Player player;

    public float maxHeight = 10;
    public float minHeight = 0;
    public float increment = 1;
    private float restraint = 0.0f;

    public GameObject[] currentCannonball;
    private bool canFire = true;
    private Coroutine reloadCoroutine;

    private void Start()
    {
        reload.SetActive(false);
        currentCannonball[0] = normalCannonball;
        currentCannonball[1] = normalCannonball;
        currentCannonball[2] = normalCannonball;
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
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCannonball(normalCannonball, 2, -1);
        if (Input.GetKeyDown(KeyCode.Alpha4) && player.numExplodeCannonballs > 0) SwitchCannonball(explodeCannonball, 3, player.numExplodeCannonballs);
        if (Input.GetKeyDown(KeyCode.Alpha5) && player.numFreezingCannonballs > 0) SwitchCannonball(freezeCannonball, 4, player.numFreezingCannonballs);
    }

    void SwitchCannonball(GameObject prefab, int inventoryIndex, int count)
    {
        if (count >= 3)
        {
            currentCannonball[0] = prefab;
            currentCannonball[1] = prefab;
            currentCannonball[2] = prefab;
            inventoryActive.UpdateActive(inventoryIndex);
        }
        else if (count == 2)
        {
            currentCannonball[0] = normalCannonball;
            currentCannonball[1] = prefab;
            currentCannonball[2] = prefab;
            inventoryActive.UpdateActive(inventoryIndex);
        }
        else if (count == 1)
        {
            currentCannonball[0] = prefab;
            currentCannonball[1] = normalCannonball;
            currentCannonball[2] = normalCannonball;
            inventoryActive.UpdateActive(inventoryIndex);
        }
        else
        {
            currentCannonball[0] = normalCannonball;
            currentCannonball[1] = normalCannonball;
            currentCannonball[2] = normalCannonball;
            inventoryActive.UpdateActive(2);
        }
    }

    void FireCannonBall()
    {
        for(int i=0; i < firing.Length; i++)
        {
            GameObject cannonBall = Instantiate(currentCannonball[i], firing[i].transform.position, Quaternion.identity, firing[i].transform);
            cannonBall.transform.rotation = firing[i].transform.rotation;
            Rigidbody rb = cannonBall.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(cannonBall.transform.TransformDirection(Vector3.forward) * power, ForceMode.Impulse);
                // rb.velocity = cannonBall.transform.TransformDirection(Vector3.forward) * power;
            }
        }
        if (reloadCoroutine != null) StopCoroutine(reloadCoroutine);
        reloadCoroutine = StartCoroutine(ReloadCannon());
        HandleDecrement();
    }

    private void HandleDecrement() 
    {
        bool explode = false;
        bool freeze = false;
        foreach (GameObject cannonball in currentCannonball)
        {
            if (cannonball == explodeCannonball)
            {
                player.numExplodeCannonballs -= 1;
                explode = true;
            }
            if (cannonball == freezeCannonball)
            {
                player.numFreezingCannonballs -= 1;
                freeze = true;
                
            }
        }
        if (explode)
        {
            SwitchCannonball(explodeCannonball, 3, player.numExplodeCannonballs);
        }
        else if (freeze)
        {
            SwitchCannonball(freezeCannonball, 4, player.numFreezingCannonballs);
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
        reload.SetActive(false);
        canFire = true;
    }

    void AimCannon()
    {
        //Reversed because cannon is upside down
        foreach (GameObject cannon in cannons)
        {
            cannon.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(minHeight, 0, 0), Quaternion.Euler(-maxHeight, 0, 0), restraint / increment);
            if (Input.GetKey(KeyCode.W))
            {
                if (restraint < increment)
                {
                    restraint += Time.deltaTime;
                }
                else
                {
                    cannon.transform.localRotation = Quaternion.Euler(-maxHeight, 0, 0);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (restraint > 0)
                {
                    restraint -= Time.deltaTime;
                }
                else
                {
                    cannon.transform.localRotation = Quaternion.Euler(minHeight, 0, 0);
                }
            }
        }
    }
}
