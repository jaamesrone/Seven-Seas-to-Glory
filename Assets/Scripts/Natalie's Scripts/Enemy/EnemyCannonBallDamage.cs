using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonBallDamage : MonoBehaviour
{
    public float damage = 10f;

    private void Start()
    {
        StartCoroutine(destroyCannonBall());
    }

    void OnCollisionEnter(Collision collision) //Currently Experimenting
    {
        if (collision.gameObject.CompareTag("PlayerShip"))
        {
            collision.gameObject.transform.root.GetComponent<PlayerShipHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator destroyCannonBall()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
