using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject[] inventory;
    public int activeIndex = 0;

    void Start()
    {
        //Deactivate all but first item
        for(int i=0; i<inventory.Length; i++)
        {
            inventory[i].SetActive(i == activeIndex);
        }
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
