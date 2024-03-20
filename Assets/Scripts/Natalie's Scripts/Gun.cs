using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float reloadTime = 3f;
    public GameObject normalBulletPrefab;
    public GameObject reloadReticle;
    public float distance;

    private bool canFire = true;
    private GameObject activeBullet;

    void Start()
    {
        reloadReticle.SetActive(false);
    }

    void Update()
    {
        if (activeBullet == null)
        {
            StopCoroutine(ReloadBullet());
            reloadReticle.SetActive(false);
            canFire = true;
        }
    }

    public void Fire()
    {
        if (canFire)
        {
            FireBullet();
            StartCoroutine(ReloadBullet());
        }
    }

    void FireBullet()
    {
        Vector3 firingPosition = transform.position + transform.forward * 1.5f;
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        Vector3 firingDirection;

        //Aim's based on mouse position
        if (Physics.Raycast(ray, out hit))
        {
            firingDirection = (hit.point - firingPosition).normalized;
        }
        else
        {
            firingDirection = ray.direction;
        }

        GameObject bullet = Instantiate(normalBulletPrefab, firingPosition, Quaternion.identity);
        activeBullet = bullet;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firingDirection * distance;

        canFire = false;
    }

    IEnumerator ReloadBullet()
    {
        reloadReticle.SetActive(true);
        float cooldownTimer = reloadTime;
        float rotationSpeed = 360f / reloadTime; 
        while (cooldownTimer > 0)
        {
            reloadReticle.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            yield return null;
            cooldownTimer -= Time.deltaTime;
        }
        canFire = true;
        reloadReticle.SetActive(false);
    }
}
