using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonBall : AmmoClass
{

    private void Start()
    {
        StartCoroutine(destroyCannonBall());
    }

    private IEnumerator destroyCannonBall()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
