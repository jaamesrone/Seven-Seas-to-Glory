using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmmoClass : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            player.transform.root.GetComponent<ShipHealth>().TakeDamage(baseDamage);
            Destroy(gameObject);
            if (addedDamage > 0)
            {
                StartCoroutine(RepeatDamage(player));
            }
            if (shotSpeedDecrease > 0)
            {
                StartCoroutine(ReduceShotSpeed(player));
            }
        }

        IEnumerator RepeatDamage(GameObject player)
        {
            float timer = 0f;
            while (timer < timeCount)
            {
                player.GetComponent<ShipHealth>().TakeDamage(addedDamage);
                yield return new WaitForSeconds(timeBetweenDamage);
                timer += timeBetweenDamage;
            }
        }

        IEnumerator ReduceShotSpeed(GameObject player)
        {
            FiringModue firingModue = player.GetComponent<FiringModue>();
            float originalSpeed = firingModue.reloadTime;
            firingModue.reloadTime += shotSpeedDecrease;
            yield return new WaitForSeconds(shotSpeedDecreaseTime);
            firingModue.reloadTime += originalSpeed;
        }
    }
}