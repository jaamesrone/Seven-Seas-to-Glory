using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FameMeter : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxMorality(int maxMorality)
    {
        slider.maxValue = maxMorality;
        slider.value = maxMorality;
        fill.color = Color.blue;
    }

    public void SetMorality(int morality)
    {
        slider.value = morality;
        if (morality <= 0)
        {
            fill.color = Color.red;
        }
        else
        {
            float normalizedMorality = (float)morality / slider.maxValue;
            fill.color = Color.Lerp(Color.red, Color.blue, normalizedMorality);
        }
    }
}
