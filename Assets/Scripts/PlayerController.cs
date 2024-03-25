using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed; 
    [SerializeField] private float acceleration; 
    [SerializeField] private float decceleration; 
    [SerializeField] private float jumpThrust; 
    [SerializeField] private float wallJumpThrust; 
    [SerializeField] private float coyoteTime;
    private float currentCoyoteTime;
    private float xInput = 0f;
    private float movement = 0f;
    private bool isJumping = false;
    private bool isFacingRight = true;
    private bool canWallJump = false;
    private bool canMove = true;

    [Header("Checks")]
    [SerializeField] private float groundCheckCastDistance;
    [SerializeField] private Vector2 groundCheckCastSize;
    public bool collectedCrystal = false;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer; 

    [Header("VFX")]
    [SerializeField] private ParticleSystem dustParticles;
    public ParticleSystem crystalParticles;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip walkSFX;
    private float walkSFXcooldown;

    [Header("Components")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.PlayerActionMap.Jump.canceled += OnJumpCanceled;
    }

    void Start()
    {
        currentCoyoteTime = coyoteTime;
        walkSFXcooldown = walkSFX.length;
    }

    void Update()
    {
        if (canMove){
            CalculateMovement();
            FlipSprite();
        };
        CoyoteTime();
        WalkSFX();

        animator.SetBool("isMoving", Mathf.Abs(xInput) > 0);
        animator.SetFloat("yVelocity", playerRb.velocity.y);
        animator.SetBool("isGrounded", IsGrounded());
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        playerRb.AddForce(Vector2.right * movement, ForceMode2D.Force);
    }

    void WalkSFX()
    {
        if (Mathf.Abs(xInput) > 0f && IsGrounded() && walkSFXcooldown <= 0f) {
            SoundFXManager.instance.PlaySoundFXClip(walkSFX, transform, 1f);
            walkSFXcooldown = walkSFX.length;
            return;
        };

        walkSFXcooldown -= 1 * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("JumpableWall")) canWallJump = true;
    }

    void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("JumpableWall")) canWallJump = false;
    }

    void Jump(float jumpForce)
    {
        isJumping = true;
        playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        dustParticles.Play();
        animator.SetTrigger("jump");
        SoundFXManager.instance.PlaySoundFXClip(jumpSFX, transform, 1f);
    }

    void CoyoteTime() {
        if (isJumping) {
            currentCoyoteTime = 0f;
            return;
        }

        if (IsGrounded()) currentCoyoteTime = coyoteTime;
        currentCoyoteTime -= 1 * Time.deltaTime;
    }

    void CalculateMovement() 
    {
        float targetSpeed = xInput * maxSpeed;
        float speedDifference = targetSpeed - playerRb.velocity.x;

        if (Mathf.Abs(xInput) == 0f) {
            movement = speedDifference * decceleration;
            return;
        };

        if (Mathf.Abs(xInput) != 0f) {
            movement = speedDifference * acceleration;
            return;
        };
    }

    void FlipSprite() 
    {
        if ((isFacingRight && xInput < 0) || (!isFacingRight && xInput > 0)) {
            playerSprite.flipX = !playerSprite.flipX;
            isFacingRight = !isFacingRight;
        }
    }
    
    bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, groundCheckCastSize, 0, -Vector2.up, groundCheckCastDistance, groundLayer);
    }

    void OnMove(InputValue value) 
    {
        xInput = value.Get<Vector2>().x;
    }

    void OnJump(InputValue value)
    {   
        if (currentCoyoteTime <= 0 && !canWallJump) return;

        if (currentCoyoteTime > 0 && !canWallJump) Jump(jumpThrust); // Normal Jump
        if (canWallJump) Jump(jumpThrust * wallJumpThrust); // Wall Jump
    }

    void OnJumpCanceled(InputAction.CallbackContext context) {
        isJumping = false;
        if (playerRb.velocity.y > 0) playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
    }

    void OnPause()
    {
        if (MenuManager.instance.isPaused) {
            MenuManager.instance.Unpause();
            return;
        };

        MenuManager.instance.Pause();
    }
    
    void OnEnable() 
    {
        playerInput.Enable();
    }
    
    void OnDisable() 
    {
        playerInput.Disable();
    }

    void OnDestroy()
    {
        playerInput.PlayerActionMap.Jump.canceled -= OnJumpCanceled;
    }
}
