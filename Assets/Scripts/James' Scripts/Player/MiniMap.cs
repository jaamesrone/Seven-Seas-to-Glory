using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player; // public variable to reference the player object

    private void LateUpdate()
    {
        Vector3 newPos = player.transform.position; // get's the player's position
        newPos.y = transform.position.y; // keep's the same y position as the minimap
        transform.position = newPos; // updates the minimap's position to follow the player

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0); // adjusts the minimap's rotation to match the player's
    }
}
