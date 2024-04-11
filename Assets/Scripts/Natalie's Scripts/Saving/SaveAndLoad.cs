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

        //Vector3 ship rotation
        Quaternion quaternion = new(data.quaternion[0], data.quaternion[1], data.quaternion[2], data.quaternion[3]);
        player.ship.transform.eulerAngles = quaternion.eulerAngles;
        player.ship.GetComponent<ShipController>().desiredRotation = quaternion.eulerAngles;
    }
}
