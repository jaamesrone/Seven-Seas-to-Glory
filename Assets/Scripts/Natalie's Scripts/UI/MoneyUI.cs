using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI moneyText;

    void Update()
    {
        moneyText.text = "$" + player.money;
    }
}
