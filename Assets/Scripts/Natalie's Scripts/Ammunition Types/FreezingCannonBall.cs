using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingCannonBall : AmmoClass
{

    private void Start()
    {
        StartCoroutine(destroyCannonBall());
    }

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

    private IEnumerator destroyCannonBall()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
