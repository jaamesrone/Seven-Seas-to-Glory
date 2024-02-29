using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    public TextMeshProUGUI popup;
    public string message = "";

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerShip"))
        {
            popup.text = message;
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerShip"))
        {
            popup.text = "";
        }
    }
}
