using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        // Move the ship
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        

        
    }
}