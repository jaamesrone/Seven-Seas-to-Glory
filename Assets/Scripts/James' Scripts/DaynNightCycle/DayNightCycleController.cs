using System.Collections;
using UnityEngine;
using TMPro;

public class DayAndNight : MonoBehaviour
{
    [Header("Gradients")]
    [SerializeField] private Gradient fogGradient;
    [SerializeField] private Gradient ambientGradient;
    [SerializeField] private Gradient directionLightGradient;
    [SerializeField] private Gradient skyboxTintGradient;

    [Header("Environmental Assets")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Material skyboxMaterial;

    [Header("Variables")]
    [SerializeField] private float dayDurationInSeconds = 60f;
    [SerializeField] private float rotationSpeed = 1f;

    private float currentTime = 0;
    public float CurrentTime => currentTime;
    public TextMeshProUGUI despawnWarning;

    // Determines if it's daytime (true) or nighttime (false)
    public bool IsDaytime => currentTime < 0.5f;

    private void Update()
    {
        updateTime();
        updateDayNightCycle();
        rotateSkybox();
        SetSpawnWarning();
    }

    private void updateTime()
    {
        currentTime += Time.deltaTime / dayDurationInSeconds;
        currentTime = Mathf.Repeat(currentTime, 1f);
    }

    private void updateDayNightCycle()
    {
        float sunPosition = Mathf.Repeat(currentTime + 0.25f, 1f);
        directionalLight.transform.rotation = Quaternion.Euler(sunPosition * 360f, 0f, 0f);

        RenderSettings.fogColor = fogGradient.Evaluate(currentTime);
        RenderSettings.ambientLight = ambientGradient.Evaluate(currentTime);
        directionalLight.color = directionLightGradient.Evaluate(currentTime);

        skyboxMaterial.SetColor("_Tint", skyboxTintGradient.Evaluate(currentTime));
    }

    private void rotateSkybox()
    {
        float currentRotation = skyboxMaterial.GetFloat("_Rotation");
        float newRotation = currentRotation + rotationSpeed * Time.deltaTime;
        newRotation = Mathf.Repeat(newRotation, 360f);
        skyboxMaterial.SetFloat("_Rotation", newRotation);
    }

    private void OnApplicationQuit()
    {
        skyboxMaterial.SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
    }

    private void SetSpawnWarning()
    {
        if(currentTime < 0.501f && currentTime >= 0.5f)
        {
            StartCoroutine(ActivateWarning("The Emperials are returning to the barracks."));
        }
        if(currentTime >= 0.998f && currentTime < 0.999f)
        {
            StartCoroutine(ActivateWarning("The Undead are returning to the depths."));
        }
    }

    private IEnumerator ActivateWarning(string warning)
    {
        despawnWarning.text = warning;
        despawnWarning.enabled = true;
        yield return new WaitForSeconds(3.5f);
        despawnWarning.enabled = false;
    }
}
