using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f; // adjust this if needed 2 is kinda high, i set it to 0.01 in the inspector.s
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Rigidbody rb;
    private Camera mainCamera;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        // Reset the mouse position to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        // adjusting the camera's forward and right vectors to calculate movement direction
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // setting the vectors onto the y to 0
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // grabbing the vector 3 variable and normalizing the speed to be constant whenever moving
        cameraForward.Normalize();
        cameraRight.Normalize();

        // setting the movement direction based of moveInput and the 2 vector3 camera orientation
        Vector3 movement = (cameraForward * moveInput.y + cameraRight * moveInput.x) * speed * Time.deltaTime;

        // Move the player
        rb.MovePosition(rb.position + movement);

        // Rotate the player based on mouse input
        Vector3 playerRotation = new Vector3(-lookInput.y, lookInput.x, 0f) * sensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(playerRotation));
    }



}
