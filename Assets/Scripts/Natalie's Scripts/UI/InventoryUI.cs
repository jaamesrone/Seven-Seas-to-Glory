using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Player player;
    public GameObject[] inventory;
    public int activeIndex = 0;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI explodeCount;
    public TextMeshProUGUI freezeCount;
    public TextMeshProUGUI bulletCount;
    public int lastCannonIndex;
    public int lastCombatIndex;

    void Start()
    {
        //Deactivate all but first item
        for(int i=0; i<inventory.Length; i++)
        {
            inventory[i].SetActive(i == activeIndex);
        }
        moneyText.text = "$" + player.money;
        explodeCount.text = player.numExplodeCannonballs.ToString();
        freezeCount.text = player.numFreezingCannonballs.ToString();
        bulletCount.text = player.numBullets.ToString();
    }

    private void Update()
    {
        moneyText.text = "$" + player.money;
        explodeCount.text = player.numExplodeCannonballs.ToString();
        freezeCount.text = player.numFreezingCannonballs.ToString();
        bulletCount.text = player.numBullets.ToString();
    }

    public void UpdateActive(int index)
    {
        //Deactivate all but first item
        if(index <= 1)
        {
            lastCombatIndex = index;
        }
        if(index >= 2)
        {
            lastCannonIndex = index;
        }
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i].SetActive(i == index);
        }
    }
}
