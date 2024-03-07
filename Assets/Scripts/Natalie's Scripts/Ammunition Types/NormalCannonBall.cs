using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCannonBall : AmmoClass
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyShip"))
        {
            GameObject enemy = collision.gameObject;
            enemy.transform.root.GetComponent<ShipHealth>().TakeDamage(baseDamage);
            Destroy(gameObject);
        }
    }
}
