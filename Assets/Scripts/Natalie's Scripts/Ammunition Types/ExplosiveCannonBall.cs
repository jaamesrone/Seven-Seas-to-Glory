using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCannonBall : AmmoClass
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyShip"))
        {
            GameObject enemy = collision.gameObject;
            enemy.transform.root.GetComponent<ShipHealth>().TakeDamage(baseDamage);
            enemy.transform.root.GetComponent<ShipHealth>().TakeIncrementalDamage(addedDamage, timeCount, timeBetweenDamage);
            Destroy(gameObject);
        }
    }
}
