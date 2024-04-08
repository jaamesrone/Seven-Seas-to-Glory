using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] position;
    public float euler;
    public int bullets;
    public int explode;
    public int freeze;
    public int money;
    public int recruits;

    //collects player data when called in SaveAndLoad
    public PlayerData(Player player)
    {
        //ammo count
        bullets = player.numBullets;
        explode = player.numExplodeCannonballs;
        freeze = player.numFreezingCannonballs;

        //money
        money = player.money;

        //recruits
        recruits = player.recruits;

        //Vector3 ship position stored in 3 float variables
        position = new float[3];
        position [0] = player.ship.transform.position.x;
        position [1] = player.ship.transform.position.y;
        position [2] = player.ship.transform.position.z;

        //Vector3 ship rotation
        euler = player.ship.transform.eulerAngles.y;
    }
}
