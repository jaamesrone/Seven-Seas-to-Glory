using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinpoint : MonoBehaviour
{
    public Transform target;
    public Camera mainCamera;
    public KeyCode toggleKey = KeyCode.M;
    public KeyCode confirmKey = KeyCode.Return;

    private bool placingCylinder;
    private GameObject cylinder;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (!placingCylinder)
            {
                placingCylinder = true;
                Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * 2.0f; // Adjust the spawn position as needed
                cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                cylinder.transform.position = spawnPosition;
            }
        }

        if (Input.GetKeyDown(confirmKey) && placingCylinder)
        {
            placingCylinder = false;
        }

        if (target != null && cylinder != null)
        {
            Vector3 targetDir = target.position - mainCamera.transform.position;
            Vector3 forward = mainCamera.transform.forward;
            float angle = Vector3.Angle(targetDir, forward);
            Vector3 cross = Vector3.Cross(targetDir, forward);
            if (cross.y < 0) angle = -angle;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
