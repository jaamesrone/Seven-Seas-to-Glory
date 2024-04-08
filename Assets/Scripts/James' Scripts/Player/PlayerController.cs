using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 1.0f;

    [Header("State")]
    public bool isBlocking = false;
    private bool isAttacking = false;
    private bool isGrounded = true;

    [Header("Components")]
    private Rigidbody rb;
    private Animator animator;
    private PlayerInput playerInput;
    public SaveAndLoad loading;

    [Header("Input")]
    private Vector2 moveInput;
    private Vector2 lookInput;

    [Header("UI")]
    public HealthBar healthBar;
    public Player player;
    public InventoryUI inventoryActive;

    [Header("Weapon Switch")]
    public Sword sword;
    public Gun gun;
    public GameObject reticleImage;
    public GameObject reload;
    private bool isUsingSword = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;

        //load save values
        loading.LoadPlayer();

        //spawn player on ship
        player.transform.position = player.spawnPoint.transform.position;
        player.transform.localEulerAngles = player.spawnPoint.transform.localEulerAngles;
    }

    private void Start()
    {
        healthBar.SetMaxHealth(player.health);
        InitializeActionControls();
        reticleImage.SetActive(false);
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayerAndCamera();
        CheckGroundedStatus();
        UpdateAnimationStates();

        if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
        {
            SwitchToSword();
        }
        if (Keyboard.current[Key.Digit2].wasPressedThisFrame)
        {
            SwitchToGun();
        }
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * speed * Time.deltaTime;
        transform.Translate(movement, Space.Self);
    }

    private void RotatePlayerAndCamera()
    {
        transform.Rotate(0f, lookInput.x * sensitivity, 0f);

        float cameraRotationX = -lookInput.y * sensitivity;
        float currentRotationX = Camera.main.transform.localEulerAngles.x;
        float newRotationX = currentRotationX + cameraRotationX;

        if (newRotationX > 180) newRotationX -= 360;
        newRotationX = Mathf.Clamp(newRotationX, -30f, 40f);

        Camera.main.transform.localEulerAngles = new Vector3(newRotationX, 0f, 0f);
    }

    private void CheckGroundedStatus()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance);
    }

    private void UpdateAnimationStates()
    {
        animator.SetBool("Running", moveInput.y > 0 && !isBlocking);
        animator.SetBool("Backwards", moveInput.y < 0 && !isBlocking);
        animator.SetBool("IsBlocking", isBlocking);
    }

    // Input System Handlers
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void SwitchToSword()
    {
        isUsingSword = true;
        reload.SetActive(false);
        reticleImage.SetActive(false);
        gun.gameObject.SetActive(false);
        sword.gameObject.SetActive(true);
        inventoryActive.UpdateActive(0);
    }

    private void SwitchToGun()
    {
        isUsingSword = false;
        sword.gameObject.SetActive(false);
        reticleImage.SetActive(true);
        gun.gameObject.SetActive(true);
        inventoryActive.UpdateActive(1);
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
        if (isUsingSword)
        {
            if (!isAttacking)
            {
                animator.SetBool("IsAttacking", true);
                isAttacking = true;
                StartCoroutine(ResetIsAttackingAfterDelay(1f));
            }
        }
        else
        {
            if(!isAttacking)
            {
                gun.Fire();
                isAttacking = true;
                StartCoroutine(ResetIsAttackingAfterDelay(gun.reloadTime));
            }
        }
    }

    public void OnBlock()
    {
        isBlocking = true;
        animator.SetBool("IsBlocking", true);
    }

    public void StopBlock()
    {
        isBlocking = false;
        animator.SetBool("IsBlocking", false);
    }

    IEnumerator ResetIsAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    private void InitializeActionControls()
    {
        var jumpAction = playerInput.actions["Jump"];
        jumpAction.performed += _ => OnJump();

        var blockAction = playerInput.actions["Block"];
        blockAction.started += _ => OnBlock();
        blockAction.canceled += _ => StopBlock();
    }
}
