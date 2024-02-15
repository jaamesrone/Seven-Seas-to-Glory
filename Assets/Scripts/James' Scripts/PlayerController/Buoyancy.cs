using UnityEngine;

public class BuoyancyWithRocking : MonoBehaviour
{
    public float rockingSpeed = 1f;
    public float rockingAngle = 10f;

    private Quaternion initialRotation;
    private Collider[] colliders;

    private void Start()
    {
        initialRotation = transform.localRotation;
        colliders = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        float angle = rockingAngle * Mathf.Sin(Time.time * rockingSpeed);
        transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, angle);
        /*
        foreach (var collider in colliders)
        {
            collider.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        */
    }
}
