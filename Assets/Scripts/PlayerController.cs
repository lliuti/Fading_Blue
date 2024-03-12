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

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer; 

    [Header("VFX")]
    [SerializeField] private ParticleSystem dustParticles;
    public ParticleSystem crystalParticles;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip walkSFX;
    [SerializeField] private AudioClip deathSFX;
    private float walkSFXcooldown;

    private Vector2 respawnPos;
    private Rigidbody2D playerRb;
    private Animator animator;
    private SpriteRenderer playerSprite;
    private PlayerInput playerInput;
    private bool isDying = false;

    public bool collectedCrystal = false;

    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.PlayerActionMap.Jump.canceled += OnJumpCanceled;
    }

    void OnEnable() 
    {
        playerInput.Enable();
    }
    
    void OnDisable() 
    {
        playerInput.Disable();
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();

        respawnPos = transform.position;
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
    
        if (transform.position.y < -9f && !isDying) Die();
    }

    void FixedUpdate()
    {
        playerRb.AddForce(new Vector2(movement, 0), ForceMode2D.Force);
        animator.SetBool("isGrounded", IsGrounded());
    }

    void OnPause()
    {
        if (MenuManager.instance.isPaused) {
            MenuManager.instance.Unpause();
        } else {
            MenuManager.instance.Pause();
        }
    }

    void WalkSFX()
    {
        if (Mathf.Abs(xInput) > 0f && walkSFXcooldown <= 0f && IsGrounded()) {
            SoundFXManager.instance.PlaySoundFXClip(walkSFX, transform, 1f);
            walkSFXcooldown = walkSFX.length;
        } else {
            walkSFXcooldown -= 1 * Time.deltaTime;
        };
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("JumpableWall")) canWallJump = true;
    }

    void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("JumpableWall")) canWallJump = false;
    }

    void OnJump(InputValue value)
    {   
        if (currentCoyoteTime <= 0 && !canWallJump) return;

        // NORMAL JUMP
        if (currentCoyoteTime > 0 && !canWallJump) {
            playerRb.AddForce(new Vector2(0, jumpThrust), ForceMode2D.Impulse);
        }

        // WALL JUMP
        if (canWallJump) {
            playerRb.AddForce(new Vector2(0, jumpThrust * wallJumpThrust), ForceMode2D.Impulse);
        }

        isJumping = true;
        dustParticles.Play();
        SoundFXManager.instance.PlaySoundFXClip(jumpSFX, transform, 1f);
        animator.SetTrigger("jump");
    }

    void OnJumpCanceled(InputAction.CallbackContext context) {
        isJumping = false;
        if (playerRb.velocity.y > 0) {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
        }
    }

    void CoyoteTime() {
        if (isJumping) {
            currentCoyoteTime = 0f;
            return;
        }

        if (IsGrounded()) currentCoyoteTime = coyoteTime;
        currentCoyoteTime -= 1 * Time.deltaTime;
    }

    void OnMove(InputValue value) 
    {
        xInput = value.Get<Vector2>().x;
    }

    void CalculateMovement() 
    {
        float targetSpeed = xInput * maxSpeed;
        float speedDifference = targetSpeed - playerRb.velocity.x;
        movement = speedDifference * acceleration;

        animator.SetBool("isMoving", Mathf.Abs(xInput) > 0);
        animator.SetFloat("yVelocity", playerRb.velocity.y);
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

    public void Die()
    {
        isDying = true;
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        SoundFXManager.instance.PlaySoundFXClip(deathSFX, transform, 1f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(deathSFX.length);
        Time.timeScale = 1f;
        playerRb.velocity = Vector2.zero;
        transform.position = respawnPos;
        isDying = false;
    }

    public void UpdateRespawnPos(Vector2 newPos)
    {
        respawnPos = newPos;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundCheckCastDistance, groundCheckCastSize);
    }

    void OnDestroy()
    {
        playerInput.PlayerActionMap.Jump.canceled -= OnJumpCanceled;
    }
}
