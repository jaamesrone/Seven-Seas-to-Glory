using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendTeleport : MonoBehaviour
{
    public GameObject shipGatheringPoint;

    // Update is called once per frame
    void Update()
    {
        // Check for the teleport command
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Teleport command received.");
            TeleportFriendsToShip();
        }
    }

    private void TeleportFriendsToShip()
    {
        // Make sure the gathering point is set
        if (shipGatheringPoint == null)
        {
            Debug.LogError("Ship gathering point is not set in the GameManager.");
            return;
        }

        foreach (var friend in FindObjectsOfType<Friend>())
        {
            friend.TeleportToShip(shipGatheringPoint);
        }
    }

}
