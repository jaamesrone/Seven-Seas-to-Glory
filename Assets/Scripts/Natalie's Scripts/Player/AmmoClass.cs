using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoClass : MonoBehaviour
{
    public float baseDamage;

    //for exploding cannonball
    public float addedDamage;
    public float timeCount;
    public float timeBetweenDamage;

    //for freezing cannonball
    public float shotSpeedDecrease;
    public float shotSpeedDecreaseTime;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;
            enemy.transform.root.GetComponent<ShipHealth>().TakeDamage(baseDamage);
            Destroy(gameObject);
            if (addedDamage > 0)
            {
                StartCoroutine(RepeatDamage(enemy));
            }
            if (shotSpeedDecrease > 0)
            {
                StartCoroutine(ReduceShotSpeed(enemy));
            }
        }
    }

    IEnumerator RepeatDamage(GameObject enemy)
    {
        float timer = 0f;
        while (timer < timeCount)
        {
            enemy.GetComponent<ShipHealth>().TakeDamage(addedDamage);
            yield return new WaitForSeconds(timeBetweenDamage);
            timer += timeBetweenDamage;
        }
    }

    IEnumerator ReduceShotSpeed(GameObject enemy)
    {
        EnemyShipAI enemyShipAI = enemy.GetComponent<EnemyShipAI>();
        float originalSpeed = enemyShipAI.shootingCooldown;
        enemyShipAI.shootingCooldown += shotSpeedDecrease;
        yield return new WaitForSeconds(shotSpeedDecreaseTime);
        enemyShipAI.shootingCooldown = originalSpeed;
    }
}
