using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallDamage : MonoBehaviour
{
    public float damage = 10f;

    void OnCollisionEnter(Collision collision) //Currently Experimenting
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<ShipHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
