using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeColor : MonoBehaviour
{
    public RawImage rawImage;
    public Slider slider;
    public float threshold = 0.5f;
    public float colorChangeSpeed = 1.0f;

    private void Update()
    {
        SetColor(slider.value);
    }

    public void SetColor(float sliderValue)
    {
        if (sliderValue < threshold)
        {
            float normalizedValue = sliderValue / threshold;
            rawImage.color = Color.Lerp(Color.red, Color.blue, normalizedValue);
        }
        else
        {
            float normalizedValue = (sliderValue - threshold) / (1 - threshold);
            rawImage.color = Color.Lerp(Color.red, Color.blue, normalizedValue);
        }
    }
}
