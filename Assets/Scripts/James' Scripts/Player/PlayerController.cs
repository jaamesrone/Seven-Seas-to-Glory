using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 1.0f; // distance for the raycast to check if grounded

    public bool isBlocking = false;
    private bool isAttacking = false;
    private bool isGrounded = true;
    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Animator playerAnimation;

    //Natalie's HealthBar
    public HealthBar healthBar;
    public Player player;
    //Natalie's save
    public GameObject saveObj;

    public void Start()
    {
        healthBar.SetMaxHealth(player.health);
        //Natalie's save script
        saveObj.GetComponent<SaveAndLoad>().LoadPlayer();
        healthBar.SetHealth(player.health);

        ActionControls();
        
        Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);

        playerAnimation = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();

        // lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FixedUpdate()
    {
        // move the player based on moveInput
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * speed * Time.deltaTime;
        transform.Translate(movement, Space.Self);

        // rotate the player based on mouse movement
        Vector3 playerRotation = new Vector3(0f, lookInput.x, 0f) * sensitivity;
        transform.Rotate(playerRotation);

        // rotate the camera based on mouse movement
        Vector3 cameraRotation = new Vector3(-lookInput.y, 0f, 0f) * sensitivity;
        float currentRotationX = Camera.main.transform.localEulerAngles.x;
        float newRotationX = currentRotationX + cameraRotation.x;
        // clamp the rotation within the specified range
        if (newRotationX > 180)// if new rotationX goes above 180 degrees (beyond straight up), subtract 360 degrees to keep it within the [-180, 180] range.
            newRotationX -= 360;
        newRotationX = Mathf.Clamp(newRotationX, -30f, 40f);
        Camera.main.transform.localEulerAngles = new Vector3(newRotationX, 0f, 0f);

        CheckGrounded();

        UpdateAnimation();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public void OnFire()
    {
        // check if the player is moving forward or backward and not currently attacking
        if ((!isAttacking || moveInput.y > 0 || moveInput.y < 0) && !isAttacking)
        {
            playerAnimation.SetBool("IsAttacking", true);
            isAttacking = true;

            StartCoroutine(ResetIsAttackingAfterDelay(1f)); //dont change that float because right now it's perfect omegalul
        }
    }

    public void OnBlock()
    {
        // trigger the block animation
        playerAnimation.SetBool("IsBlocking", true);
        isBlocking = true;
    }

    public void StopBlock()
    {
        // stopping the block animation
        playerAnimation.SetBool("IsBlocking", false);
        isBlocking = false;
    }

    IEnumerator ResetIsAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerAnimation.SetBool("IsAttacking", false);
        isAttacking = false;
    }

    public void CheckGrounded()
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

    public void UpdateAnimation()
    {
        // checking if the block action is currently performed
        bool isBlocking = BlockActionIsPerformed();

        // checking if the player is moving forward or backward
        bool isMovingForward = moveInput.y > 0;
        bool isMovingBackward = moveInput.y < 0;

        // making sure the overall movement state of the player
        bool isMoving = isMovingForward || isMovingBackward;

        // setting my animation parameters based on input
        playerAnimation.SetBool("Running", isMovingForward && !isBlocking);
        playerAnimation.SetBool("Backwards", isMovingBackward && !isBlocking);
        playerAnimation.SetBool("IsBlocking", isBlocking);

        //if the player is not moving or blocking, set the idle animation
        if (!isMoving && !isBlocking)
        {
            playerAnimation.SetBool("Idle", true);
        }
        else
        {
            playerAnimation.SetBool("Idle", false);
        }
    }

    public bool BlockActionIsPerformed()
    {
        // get the block action map
        var blockActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");

        // check if the block action is currently performed
        return blockActionMap["Block"].ReadValue<float>() > 0.5f;
    }

    public void ActionControls()
    {
        InputAction jumpAction = GetComponent<PlayerInput>().actions.FindAction("Jump");
        jumpAction.performed += ctx => OnJump();
        InputAction blockAction = GetComponent<PlayerInput>().actions.FindAction("Block");
        blockAction.started += ctx => OnBlock();
        blockAction.canceled += ctx => StopBlock();
    }
}
