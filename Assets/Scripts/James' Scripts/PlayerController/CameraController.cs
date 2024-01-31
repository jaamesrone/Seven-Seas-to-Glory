using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        // Set the camera's rotation to look straight ahead
        transform.rotation = Quaternion.identity; // Quaternion.identity represents no rotation
    }
}
