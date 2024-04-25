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
    public float bulletForce;

    [Header("State")]
    public bool isBlocking = false;
    private bool isAttacking = false;
    public bool isAiming = false;
    private bool isGrounded = true;
    private bool isUsingGun = false;
    private bool isUsingSword = true;

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
    public GameObject normalBulletPrefab;
    public GameObject firingPoint;
    public GameObject reticleImage;
    public GameObject reload;

    public AudioSource gunFire;
    public AudioSource swordSwing;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;

        //load save values
        //loading.LoadPlayer();

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

        if (Input.GetKeyDown(KeyCode.Alpha1) || player.numBullets <= 0)
        {
            SwitchToSword();
        }
        if ((Input.GetKeyDown(KeyCode.Alpha2) && player.numBullets > 0))
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
        animator.SetBool("IsAiming", isAiming);
    }

    private void SwitchToSword()
    {
        isUsingSword = true;
        isUsingGun = false;
        reload.SetActive(false);
        reticleImage.SetActive(false);
        gun.gameObject.SetActive(false);
        sword.gameObject.SetActive(true);
        inventoryActive.UpdateActive(0);
    }

    private void SwitchToGun()
    {
        isUsingSword = false;
        isUsingGun = true;
        sword.gameObject.SetActive(false);
        reticleImage.SetActive(true);
        gun.gameObject.SetActive(true);
        inventoryActive.UpdateActive(1);
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
                swordSwing.Play();
                StartCoroutine(ResetIsAttackingAfterDelay(1f));
            }
        }
        else if(isUsingGun)
            {
                gunFire.Play();
                GameObject bullet = Instantiate(normalBulletPrefab, firingPoint.transform.position, firingPoint.transform.rotation);
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                if (bulletRigidbody != null)
                {
                    bulletRigidbody.AddForce(bullet.transform.forward * bulletForce, ForceMode.Impulse);
                }
                player.numBullets -= 1;
                isAttacking = true;
                StartCoroutine(ResetBullet(0.1f));
            }
        if (player.numBullets <= 0)
        {
            StopAim();
            SwitchToSword();
        }
    }

    private IEnumerator ResetBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        
    }

    public void OnBlock()
    {
        if (isUsingSword)
        {
            isBlocking = true;
            animator.SetBool("IsBlocking", true);
        }
    }

    public void StopBlock()
    {
        if (isUsingSword)
        {
            isBlocking = false;
            animator.SetBool("IsBlocking", false);
        }
    }

    public void OnAim()
    {
        if (isUsingGun)
        {
            isAiming = true;
            animator.SetBool("IsAiming", true);
        }
    }

    public void StopAim()
    {
        if(isUsingGun)
        {
            isAiming = false;
            animator.SetBool("IsAiming", false);
        }
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

        var aimAction = playerInput.actions["Aim"];
        aimAction.performed += _ => OnAim();
        aimAction.canceled += _ => StopAim();
    }
}
