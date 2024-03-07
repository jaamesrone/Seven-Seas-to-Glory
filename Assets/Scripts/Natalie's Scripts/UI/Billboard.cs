using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject canvas;
    private Transform camTransform;

    void Start()
    {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            camTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("Main Camera not found.");
        }
    }
    void LateUpdate()
    {
        if (canvas == null || camTransform == null)
        {
            return;
        }
        canvas.transform.LookAt(transform.position + camTransform.forward);
    }
}
