using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCannonBall : AmmoClass
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
        }
    }

    private IEnumerator destroyCannonBall()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
