using UnityEngine;
using UnityEngine.UI;

public class FameInfamyMeter : MonoBehaviour
{
    public Slider meterSlider;

    private float currentFame;
    private float currentInfamy;
    private float maxFame = 100f;
    private float maxInfamy = 100f;

    void Start()
    {
        currentFame = 50f; // Starting values
        currentInfamy = 50f; // Starting values
        UpdateMeter();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangeFame(5f);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeInfamy(5f);
        }
    }

    void UpdateMeter()
    {
        float famePercentage = currentFame / maxFame;
        float infamyPercentage = currentInfamy / maxInfamy;

        // Set the value of the Slider based on the combined fame and infamy levels
        meterSlider.value = (famePercentage - infamyPercentage + 1f) / 2f;

        // Set the fill color of the Slider based on its normalized value
        if (meterSlider.normalizedValue <= 0.5f)
        {
            meterSlider.fillRect.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.white, meterSlider.normalizedValue * 2);
        }
        else
        {
            meterSlider.fillRect.GetComponent<Image>().color = Color.Lerp(Color.white, Color.red, (meterSlider.normalizedValue - 0.5f) * 2);
        }
    }

    public void ChangeFame(float amount)
    {
        float infamyChange = -amount;
        currentFame = Mathf.Clamp(currentFame + amount, 0f, maxFame);
        currentInfamy = Mathf.Clamp(currentInfamy + infamyChange, 0f, maxInfamy);
        UpdateMeter();
    }

    public void ChangeInfamy(float amount)
    {
        float fameChange = -amount;
        currentInfamy = Mathf.Clamp(currentInfamy + amount, 0f, maxInfamy);
        currentFame = Mathf.Clamp(currentFame + fameChange, 0f, maxFame);
        UpdateMeter();
    }
}