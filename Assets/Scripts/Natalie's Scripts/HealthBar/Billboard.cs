using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject canvas;
    public Transform cam = null;

    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }
    void LateUpdate()
    {
        canvas.transform.LookAt(transform.position + cam.forward);
    }
}
