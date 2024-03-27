using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject[] inventory;
    public int activeIndex = 0;

    public void Start()
    {
        //Deactivate all but first item
        for(int i=0; i<inventory.Length; i++)
        {
            inventory[i].SetActive(i == activeIndex);
        }
    }
    void Update()
    {
        //Check for which number key is pressed
        for (int i=0; i<inventory.Length; i++)
        {
            //Ensure the number pressed is within inventory range
            if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                //Deactivate all but corresponding index
                for (int j=0; j<inventory.Length; j++)
                {
                    inventory[j].SetActive(j == i);
                }
                activeIndex = i;
                break;
            }
        }
    }
}
