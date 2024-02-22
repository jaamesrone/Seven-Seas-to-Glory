using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    public Player player;

    public void SavePLayer()
    {
        Saving.SavePlayer(player);
    }

    public void LoadPlayer()
    {
        PlayerData data = Saving.LoadPlayer();

        //set player health
        player.health = data.health;

        //set player position
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        player.transform.position = position;
    }
}
