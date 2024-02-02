using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 1.0f; // Distance for the raycast to check if grounded

    private bool isAttacking = false;
    private bool isGrounded = true;
    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector2 lookInput;

    Animator playerAnimation;


    void Start()
    {
        InputAction jumpAction = GetComponent<PlayerInput>().actions.FindAction("Jump");
        jumpAction.performed += ctx => OnJump();

        playerAnimation = GetComponent<Animator>();

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
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnFire()
    {
        // Check if the player is moving forward or backward and not currently attacking
        if ((moveInput.y > 0 || moveInput.y < 0) && !isAttacking)
        {
            // Set the "IsAttacking" parameter to true to initiate the attack
            playerAnimation.SetBool("IsAttacking", true);
            isAttacking = true;

            // Automatically reset the "IsAttacking" parameter after a delay
            StartCoroutine(ResetIsAttackingAfterDelay(1f)); // Adjust the delay as needed
        }
    }

    IEnumerator ResetIsAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset the "IsAttacking" parameter to false after the specified delay
        playerAnimation.SetBool("IsAttacking", false);
        isAttacking = false; // Allow the player to attack again
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

        // Check if the player is grounded
        CheckGrounded();

        UpdateAnimation();
    }

    void CheckGrounded()
    {
        // Perform a raycast to check if the player is grounded
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundCheckDistance))
        {
            isGrounded = true;
            Debug.Log("Player is grounded");
        }
        else
        {
            isGrounded = false;
            Debug.Log("Player is airborne");
        }
    }

    void UpdateAnimation()
    {

        // Check if the player is moving forward or backward
        bool isMovingForward = moveInput.y > 0;
        bool isMovingBackward = moveInput.y < 0;

        // Set the "Running" parameter in the animator based on whether the player is moving forward or backward
        playerAnimation.SetBool("Running", isMovingForward);

        // Set the "Backwards" parameter in the animator based on whether the player is moving backward
        playerAnimation.SetBool("Backwards", isMovingBackward);
    }


}
