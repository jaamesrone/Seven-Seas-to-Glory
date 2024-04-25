using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipFloating : MonoBehaviour
{
    public int maxForce;
    public float waterLevel;
    public float dampingFactor = 0.1f;

    private void FixedUpdate()
    {
            float distanceBelowWater = waterLevel - transform.position.y;
            Vector3 buoyantForce = Vector3.up * distanceBelowWater * maxForce;

            Vector3 dampingForce = -GetComponent<Rigidbody>().velocity * dampingFactor;

            Vector3 netForce = buoyantForce + dampingForce;

            GetComponent<Rigidbody>().AddForce(netForce);
    }
}
