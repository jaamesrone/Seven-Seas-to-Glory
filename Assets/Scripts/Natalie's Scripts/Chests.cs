using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chests : MonoBehaviour
{
    public Player player;
    public int maxMoney = 100;
    public int minMoney = 10;
    public int maxCannonballChance = 4;
    public int maxCannonballGain = 6;

    private bool canOpen = false;

    private void Update()
    {
        if(canOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                player.money += GetRandomNum();
                SetAmmoDrop();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter()
    {
        canOpen = true;
    }

    private void OnTriggerExit()
    {
        canOpen = false;
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
