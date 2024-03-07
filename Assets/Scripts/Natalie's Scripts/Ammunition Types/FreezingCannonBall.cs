using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingCannonBall : AmmoClass
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyShip"))
        {
            GameObject enemy = collision.gameObject;
            Destroy(gameObject);
            enemy.transform.root.GetComponent<ShipHealth>().TakeDamage(baseDamage);
            enemy.transform.root.GetComponent<EnemyShipAI>().slowReload(shotSpeedDecrease, shotSpeedDecreaseTime);

        }
    }
}
