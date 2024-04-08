using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    public Player player;

    public void SavePlayer()
    {
        Saving.SavePlayer(player);
    }

    public void LoadPlayer()
    {
        PlayerData data = Saving.LoadPlayer();

        ///ammo count
        player.numBullets = data.bullets;
        player.numExplodeCannonballs = data.explode;
        player.numFreezingCannonballs = data.freeze;

        //money
        player.money = data.money;

        //recruits
        player.recruits = data.recruits;

        //Vector3 ship position stored in 3 float variables
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        player.ship.transform.position = position;

        //spawns player at spawn point
        Vector3 spawn;
        spawn.x = data.spawn[0];
        spawn.y = data.spawn[1];
        spawn.z = data.spawn[2];
        player.transform.position = spawn;
    }

    public void LoadShip()
    {
        PlayerData data = Saving.LoadPlayer();

        //Vector3 ship rotation
        player.ship.transform.rotation = Quaternion.Euler(0f, data.euler, 0f);
    }
}
