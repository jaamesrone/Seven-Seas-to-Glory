using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public int bulletPrice = 10;
    public int freezingPrice = 20;
    public int explodingPrice = 25;

    public Player player;

    public TextMeshProUGUI purchaseConfirm;

    public void BuyBullet()
    {
        if(CheckPrice(bulletPrice))
        {
            player.money -= bulletPrice;
            player.numBullets += 1;
        }
    }

    public void BuyFreeze()
    {
        if (CheckPrice(freezingPrice))
        {
            player.money -= freezingPrice;
            player.numFreezingCannonballs += 1;
        }
    }

    public void BuyExplode()
    {
        if (CheckPrice(explodingPrice))
        {
            player.money -= explodingPrice;
            player.numExplodeCannonballs += 1;
        }
    }

    //check if item can be purchased
    private bool CheckPrice(int price)
    {
        bool value = price <= player.money;

        if (value)
        {
            purchaseConfirm.text = "-$" + price.ToString();
        }
        else
        {
            purchaseConfirm.text = "You don't have enough money";
        }

        if (!purchaseConfirm.enabled)
        {
            StartCoroutine(DisplayPurchase());
        }

        return value;
    }

    private IEnumerator DisplayPurchase()
    {
        purchaseConfirm.enabled = true;
        yield return new WaitForSecondsRealtime(3f);
        purchaseConfirm.enabled = false;
    }
}
