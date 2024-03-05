using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject canvas;
    public Transform cam = null;

    void Start()
    {
        if (cam == null)
        {
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        }
    }
    void LateUpdate()
    {
        if (cam != null)
        {
            canvas.transform.LookAt(transform.position + cam.forward);
        }
    }
}
