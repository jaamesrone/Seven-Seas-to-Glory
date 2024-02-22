using UnityEngine;

public class BuoyancyWithRocking : MonoBehaviour
{
    public float rockingSpeed = 1f;
    public float rockingAngle = 10f;

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
     //   Debug.Log("disable?");
        float angle = rockingAngle * Mathf.Sin(Time.time * rockingSpeed);
        transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, angle);
        
    }
}
