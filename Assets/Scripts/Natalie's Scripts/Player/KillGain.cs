using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGain : MonoBehaviour
{
    public int maxMoney = 100;
    public int minMoney = 10;
    public int maxCannonballChance = 4;
    public int maxCannonballGain = 6;

    public void Spare()
    {
        gameObject.GetComponent<Player>().money += GetRandomNum();
        SetAmmoDrop();
    }

    public void Kill()
    {
        gameObject.GetComponent<Player>().money += GetRandomNum() + 100;
        SetAmmoDrop();
    }

    public void ShipSink()
    {
        gameObject.GetComponent<Player>().money += GetRandomNum();
        SetAmmoDrop();
    }

    void SetAmmoDrop()
    {
        int rand = Random.Range(0, maxCannonballChance);
        if (rand == 0)
        {
            gameObject.GetComponent<Player>().numExplodeCannonballs += Random.Range(1, maxCannonballGain + 1);
        }
        if (rand == 1)
        {
            gameObject.GetComponent<Player>().numFreezingCannonballs += Random.Range(1, maxCannonballGain + 1);
        }
    }

    private int GetRandomNum()
    {
        return Random.Range(minMoney, maxMoney + 1);
    }
}