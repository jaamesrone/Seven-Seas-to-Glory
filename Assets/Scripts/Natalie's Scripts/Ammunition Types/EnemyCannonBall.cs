using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonBall : AmmoClass
{

    private void Start()
    {
        StartCoroutine(destroyCannonBall());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            GameObject player = collision.gameObject;
            player.transform.root.GetComponent<PlayerShipHealth>().TakeDamage(baseDamage);
            Destroy(gameObject);
        }
    }

    private IEnumerator destroyCannonBall()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
