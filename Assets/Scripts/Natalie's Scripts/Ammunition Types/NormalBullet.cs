using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : AmmoClass
{
    private void Start()
    {
        StartCoroutine(destroyBullet());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;
            Destroy(gameObject);
            enemy.transform.root.GetComponent<Pirate>().TakeDamage(baseDamage);
        }
    }

    private IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
