using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Player player;
    public GameObject[] inventory;
    public int activeIndex = 0;
    public TextMeshProUGUI explodeCount;
    public TextMeshProUGUI freezeCount;

    void Start()
    {
        //Deactivate all but first item
        for(int i=0; i<inventory.Length; i++)
        {
            inventory[i].SetActive(i == activeIndex);
        }
    }

    private void Update()
    {
        explodeCount.text = player.numExplodeCannonballs.ToString();
        freezeCount.text = player.numFreezingCannonballs.ToString();
    }

    public void UpdateActive(int index)
    {
        //Deactivate all but first item
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i].SetActive(i == index);
        }
    }
}
