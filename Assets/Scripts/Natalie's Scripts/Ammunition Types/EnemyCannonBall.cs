using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonBall : AmmoClass
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            GameObject player = collision.gameObject;
            player.transform.root.GetComponent<ShipHealth>().TakeDamage(baseDamage);
            Destroy(gameObject);
        }
    }
}
