using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("popup"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("popup"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
