using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonBallDamage : MonoBehaviour
{
    public float damage = 10f;

    void OnCollisionEnter(Collision collision) //Currently Experimenting
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            collision.gameObject.transform.root.GetComponent<PlayerShipHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
