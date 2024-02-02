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

        // lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;

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
        // check if the player is moving forward or backward and not currently attacking
        if ((!isAttacking||moveInput.y > 0 || moveInput.y < 0) && !isAttacking )
        {
            playerAnimation.SetBool("IsAttacking", true);
            isAttacking = true;

            StartCoroutine(ResetIsAttackingAfterDelay(1f)); // Adjust the delay as needed
        }
    }

    IEnumerator ResetIsAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        playerAnimation.SetBool("IsAttacking", false);
        isAttacking = false; 
    }


    void FixedUpdate()
    {
        // move the player based on moveInput
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * speed * Time.deltaTime;
        transform.Translate(movement, Space.Self);

        // rotate the player based on mouse movement
        Vector3 playerRotation = new Vector3(0f, lookInput.x, 0f) * sensitivity;
        transform.Rotate(playerRotation);

        // rotate the camera based on mouse movement
        Vector3 cameraRotation = new Vector3(-lookInput.y, 0f, 0f) * sensitivity;
        Camera.main.transform.Rotate(cameraRotation);

        
        CheckGrounded();

        UpdateAnimation();
    }

    void CheckGrounded()
    {
        // checks to see if i am on the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void UpdateAnimation()
    {

        // checks to see if the player is moving forward or backward
        bool isMovingForward = moveInput.y > 0;
        bool isMovingBackward = moveInput.y < 0;

        
        playerAnimation.SetBool("Running", isMovingForward);

        playerAnimation.SetBool("Backwards", isMovingBackward);
    }


}
