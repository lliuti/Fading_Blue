using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed; 
    [SerializeField] private float acceleration; 
    [SerializeField] private float jumpThrust; 
    [SerializeField] private float wallJumpThrust; 

    [Header("Checks")]
    [SerializeField] private float groundCheckCastDistance;
    [SerializeField] private Vector2 groundCheckCastSize;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer; 

    private Rigidbody2D playerRb;
    private Animator animator;
    private SpriteRenderer playerSprite;
    private float xInput = 0f;
    private float movement = 0f;
    private bool isFacingRight = true;
    private bool canWallJump = false;
    private Vector2 respawnPos;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        respawnPos = transform.position;
    }

    void Update()
    {
        HandleInput();
        CalculateMovement();
        FlipSprite();
        if (transform.position.y < -5f) Die();
    }

    void FixedUpdate()
    {
        Move();
        animator.SetBool("isGrounded", IsGrounded());
    }

    void Die()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1f;
        playerRb.velocity = Vector2.zero;
        transform.position = respawnPos;
    }

    public void UpdateRespawnPos(Vector2 newPos)
    {
        respawnPos = newPos;
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded()) Jump();
        if (Input.GetButtonDown("Jump") && canWallJump) WallJump();
        if (Input.GetButtonUp("Jump")) JumpCut();
    }

    void WallJump()
    {
        animator.SetTrigger("jump");
        playerRb.AddForce(new Vector2(0, jumpThrust * wallJumpThrust), ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("JumpableWall")) canWallJump = true;
    }

    void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("JumpableWall")) canWallJump = false;
    }

    bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, groundCheckCastSize, 0, -Vector2.up, groundCheckCastDistance, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundCheckCastDistance, groundCheckCastSize);
    }

    void Jump()
    {
        animator.SetTrigger("jump");
        playerRb.AddForce(new Vector2(0, jumpThrust), ForceMode2D.Impulse);
    }

    void JumpCut()
    {
        if (playerRb.velocity.y < 0) return;
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
    }

    void CalculateMovement() 
    {
        xInput = Input.GetAxisRaw("Horizontal");      
        float targetSpeed = xInput * maxSpeed;
        float speedDifference = targetSpeed - playerRb.velocity.x;
        movement = speedDifference * acceleration;

        animator.SetBool("isMoving", Mathf.Abs(xInput) > 0);
        animator.SetFloat("yVelocity", playerRb.velocity.y);
    }

    void Move() 
    {
        playerRb.AddForce(new Vector2(movement, 0), ForceMode2D.Force);
    }

    void FlipSprite() 
    {
        if ((isFacingRight && xInput < 0) || (!isFacingRight && xInput > 0)) {
            playerSprite.flipX = !playerSprite.flipX;
            isFacingRight = !isFacingRight;
        }
    }
}
