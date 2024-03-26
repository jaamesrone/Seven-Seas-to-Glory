using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGain : MonoBehaviour
{
    public Player player;
    public int upperBound;
    public int lowerBound;
    
    public void Spare()
    {
        player.money += GetRandomNum();
    }

    public void Kill()
    {
        player.money += GetRandomNum() + 100;
    }

    public void ShipSink()
    {
        player.money += GetRandomNum();
    }

    private int GetRandomNum()
    {
        return Random.Range(lowerBound, upperBound);
    }
}
