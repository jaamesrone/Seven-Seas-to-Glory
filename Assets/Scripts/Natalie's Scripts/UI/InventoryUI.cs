using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Player player;
    public GameObject[] inventoryActiveIndicator;
    public GameObject[] inventoryItems;
    public int activeIndex = 0;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI explodeCount;
    public TextMeshProUGUI freezeCount;
    public TextMeshProUGUI bulletCount;
    public int lastCannonIndex = 2;
    public int lastCombatIndex = 0;

    void Start()
    {
        //Deactivate indicator on all but first item
        for(int i=0; i<inventoryActiveIndicator.Length; i++)
        {
            inventoryActiveIndicator[i].SetActive(i == activeIndex);
        }
        //Deactivate all but sword and gun
        SwitchInventory(true);
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
        for (int i = 0; i < inventoryActiveIndicator.Length; i++)
        {
            inventoryActiveIndicator[i].SetActive(i == index);
        }
    }

    public void SwitchInventory(bool inCharacter)
    {
        if(inCharacter)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                //deactivated all but sword and gun
                if (i != 0 && i != 1) 
                {
                    inventoryItems[i].SetActive(false);
                }
                else
                {
                    inventoryItems[i].SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                //deactivated all but cannonballs
                if (i == 0 || i == 1) 
                {
                    inventoryItems[i].SetActive(false);
                }
                else
                {
                    inventoryItems[i].SetActive(true);
                }
            }
        }
    }
}
