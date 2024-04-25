using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FameMeter : MonoBehaviour
{
    public int status = 0;
    public int recruit = 0;
    public int maxGood = 10;
    public int maxBad = -10;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI recruitText;

    private Color neutralColor = Color.white;
    private Color maxGoodColor = Color.blue;
    private Color maxBadColor = Color.red;

    public AudioSource goodSource;
    public AudioClip activateGood;

    public AudioSource badSource;
    public AudioClip activateBad;

    private void Update()
    {
        // Calculate the color based on the status value
        float ratio = Mathf.InverseLerp(maxBad, maxGood, status);
        Color lerpedColor = Color.Lerp(maxBadColor, maxGoodColor, ratio);

        // Update the text color
        statusText.color = lerpedColor;

       
      

        // Update the text based on the status value
        if (status >= 1 && status <= 5)
        {
            statusText.text = "Status: Kind Pirate";
           
        }
        else if (status >= 6 && status <= 8)
        {
            statusText.text = "Status: Merciful Pirate";
        }
        else if (status >= 9 && status <= 10)
        {
            statusText.text = "Status: God Pirate";
        }
        else if (status >= -5 && status <= -1)
        {
            statusText.text = "Status: Unkind Pirate";

        }
        else if (status >= -8 && status <= -6)
        {
            statusText.text = "Status: Merciless Pirate";
        }
        else if (status >= -10 && status <= -9)
        {
            statusText.text = "Status: Devil Pirate";
        }
        else
        {
            statusText.text = "Status: Neutral Pirate";
        }

        // Update the recruit text
        recruitText.text = "Recruit: " + recruit.ToString();
    }

    // For testing purposes, you can use these methods to change the status value
    public void IncreaseStatus()
    {
        if (status < maxGood)
        {
            status++;
            recruit++;
            GoodActivateSound();
        }
    }

    public void DecreaseStatus()
    {
        if (status > maxBad)
        {
            status--;
            BadActivateSound();
        }
    }


    void GoodActivateSound()
    {
        if (goodSource != null && activateGood != null)
        {
            goodSource.PlayOneShot(activateGood);
        }
    }

    void BadActivateSound()
    {
        if (badSource != null && activateBad != null)
        {
            goodSource.PlayOneShot(activateBad);
        }
    }
}
