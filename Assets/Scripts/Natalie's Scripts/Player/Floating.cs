using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Floating : MonoBehaviour
{
    public int maxForce;
    public float waterLevel;
    public float dampingFactor = 0.1f;
    public TextMeshProUGUI returnToShip;
    public GameObject shipReturn;

    public bool floating = false;

    private void FixedUpdate()
    {
        if(transform.position.y < waterLevel)
        {
            float distanceBelowWater = waterLevel - transform.position.y;
            Vector3 buoyantForce = Vector3.up * distanceBelowWater * maxForce;

            Vector3 dampingForce = -GetComponent<Rigidbody>().velocity * dampingFactor;

            Vector3 netForce = buoyantForce + dampingForce;

            GetComponent<Rigidbody>().AddForce(netForce);

            floating = true;
            returnToShip.enabled = true;
        }

        if(floating == true && Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.transform.position = shipReturn.transform.position;
            floating = false;
            returnToShip.enabled = false;
        }
    }
}