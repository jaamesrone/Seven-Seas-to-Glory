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
}
