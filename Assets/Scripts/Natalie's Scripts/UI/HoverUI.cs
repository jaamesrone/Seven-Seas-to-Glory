using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverUI : MonoBehaviour
{
    public string description;
    public GameObject descriptionPanel;
    public TextMeshProUGUI text;

    private void OnMouseEnter()
    {
        text.text = description;
        descriptionPanel.SetActive(true);
    }
    private void OnMouseExit()
    {
        descriptionPanel.SetActive(false);
    }
}
