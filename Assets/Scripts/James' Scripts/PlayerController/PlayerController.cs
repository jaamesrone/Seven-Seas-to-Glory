using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 10f;

    private bool isGrounded = true;
    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector2 lookInput;

    void Start()
    {
        InputAction jumpAction = GetComponent<PlayerInput>().actions.FindAction("Jump");
        jumpAction.performed += ctx => OnJump();


        rb = GetComponent<Rigidbody>();

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Set the initial rotation to look straight
        transform.rotation = Quaternion.identity;
        Camera.main.transform.localRotation = Quaternion.identity;

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void OnJump()
    {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        
    }



    void GroundCheck()
    {
        // Use transform.TransformDirection to convert local forward to world space forward
        isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), 0.1f);
        Debug.Log("Is Grounded: " + isGrounded);
    }
 


    void FixedUpdate()
    {

        // Move the player based on moveInput
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * speed * Time.deltaTime;
        transform.Translate(movement, Space.Self);

        // Rotate the player based on mouse input
        Vector3 playerRotation = new Vector3(0f, lookInput.x, 0f) * sensitivity;
        transform.Rotate(playerRotation);

        // Rotate the camera based on mouse input
        Vector3 cameraRotation = new Vector3(-lookInput.y, 0f, 0f) * sensitivity;
        Camera.main.transform.Rotate(cameraRotation);


    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 0.1f);
    }
}
