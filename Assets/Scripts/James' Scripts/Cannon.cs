using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannon;
    public GameObject firing;

    public float power;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireCannonBall();
        }
    }

    void FireCannonBall()
    {
        GameObject cannonBall = Instantiate(cannon, firing.transform.position, Quaternion.identity,firing.transform);
        cannonBall.transform.rotation = firing.transform.rotation;
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();

        if (rb != null)
        {

             rb.AddForce(cannonBall.transform.TransformDirection(Vector3.forward) * power, ForceMode.Impulse);
           // rb.velocity = cannonBall.transform.TransformDirection(Vector3.forward) * power;
        }
    }
}
