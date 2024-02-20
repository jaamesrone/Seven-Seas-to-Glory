using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallDamage : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) //Currently Experimenting
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
