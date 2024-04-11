using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : AmmoClass
{
    private void Start()
    {
        StartCoroutine(destroyBullet());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has an enemy script
        Pirate enemy = other.GetComponent<Pirate>();
        SharkPirate sharkPirate = other.GetComponent<SharkPirate>();
        ImperialPirate royal = other.GetComponent<ImperialPirate>();
        SkeletonPirate zombie = other.GetComponent<SkeletonPirate>();

        if (enemy != null)
        {
            // Deal damage to the enemy
            enemy.TakeDamage(baseDamage);
            Debug.Log("Hitting pirate?");
        }
        else if (sharkPirate != null)
        {
            // Deal damage to the enemy
            sharkPirate.TakeDamage(baseDamage);
            Debug.Log("Hitting sharkpirate?");
        }
        else if (royal != null)
        {
            // Deal damage to the enemy
            royal.TakeDamage(baseDamage);
            Debug.Log("Hitting royal?");
        }
        else if (zombie != null)
        {
            // Deal damage to the enemy
            zombie.TakeDamage(baseDamage);
            Debug.Log("Hitting zombie?");
        }
    }

    private IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
