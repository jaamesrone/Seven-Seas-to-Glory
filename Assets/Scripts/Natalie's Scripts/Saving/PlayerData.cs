using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //player data
    public float[] position;
    public float health;

    //collects player data when called in SaveAndLoad
    public PlayerData(Player player)
    {
        //Vector3 stored in 3 float variables
        position = new float[3];
        position [0] = player.transform.position.x;
        position [1] = player.transform.position.y;
        position [2] = player.transform.position.z;

        //player health
        health = player.health;
    }
}
