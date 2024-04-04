using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGain : MonoBehaviour
{
    public Player player;
    public int maxMoney;
    public int minMoney;
    public int maxCannonballChance;
    public int maxCannonballGain;
    
    public void Spare()
    {
        player.money += GetRandomNum();
        SetAmmoDrop();
    }

    public void Kill()
    {
        player.money += GetRandomNum() + 100;
        SetAmmoDrop();
    }

    public void ShipSink()
    {
        player.money += GetRandomNum();
        SetAmmoDrop();
    }

    void SetAmmoDrop()
    {
        int rand = Random.Range(0, maxCannonballChance);
        if (rand == 0)
        {
            player.numExplodeCannonballs += Random.Range(1, maxCannonballGain + 1);
        }
        if (rand == 1)
        {
            player.numFreezingCannonballs += Random.Range(1, maxCannonballGain + 1);
        }
    }

    private int GetRandomNum()
    {
        return Random.Range(minMoney, maxMoney + 1);
    }
}
