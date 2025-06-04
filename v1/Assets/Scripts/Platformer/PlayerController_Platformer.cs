using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController_Platformer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float jumpCutMultiplier = 0.5f;
    public float fallMultiplier = 2.5f;

    [Header("Ground Detection")]
    public LayerMask groundLayer; // Assign your tilemap/ground layer in Inspector
    public float groundRayLength = 0.3f; // Increased to better detect ground
    public Vector2 groundRayOffset = new Vector2(0.3f, 0); // Side rays offset from center
    public float feetOffset = 0.5f; // Offset from center to feet - adjust based on your character height

    [Header("Collectibles")]
    public List<GameObject> collectibles; // Assign in Inspector
    public int collectiblePoints = 1;

    [Header("Audio")]
    public AudioSource jumpSound; // Assign in Inspector
    public AudioSource normalCollectSound; // Assign in Inspector
    public AudioSource specialCollectSound; // Assign in Inspector

    private Rigidbody2D rb;
    public Animator animator;
    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private bool facingRight = true;
    private PlatformerManager platformerManager;
    private Collider2D playerCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        platformerManager = FindObjectOfType<PlatformerManager>();
        playerCollider = GetComponent<Collider2D>();
        wasGroundedLastFrame = isGrounded;
    }



    void Update()
    {
        // Check if player is on the ground
        CheckGrounded();

        // Track grounded state change for jump sound
        if (!wasGroundedLastFrame && isGrounded)
        {
            // Landing - optional landing sound could go here
        }
        wasGroundedLastFrame = isGrounded;

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", Mathf.Abs(rb.velocity.x) > 0.01f);
        animator.SetBool("isJump", !isGrounded);

        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            PlaySound(jumpSound);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void CheckGrounded()
    {
        // Start position for raycasts - at the character's feet
        Vector2 rayOrigin;

        if (playerCollider != null)
        {
            // Use the bottom of the collider if available
            rayOrigin = new Vector2(transform.position.x, playerCollider.bounds.min.y + 0.05f);
        }
        else
        {
            // Fallback - use transform position minus feet offset
            rayOrigin = new Vector2(transform.position.x, transform.position.y - feetOffset);
        }

        // Center ray
        RaycastHit2D centerHit = Physics2D.Raycast(rayOrigin, Vector2.down, groundRayLength, groundLayer);

        // Left ray
        RaycastHit2D leftHit = Physics2D.Raycast(
            rayOrigin - new Vector2(groundRayOffset.x, 0),
            Vector2.down, groundRayLength, groundLayer);

        // Right ray
        RaycastHit2D rightHit = Physics2D.Raycast(
            rayOrigin + new Vector2(groundRayOffset.x, 0),
            Vector2.down, groundRayLength, groundLayer);

        // Player is grounded if any ray hits the ground
        isGrounded = centerHit.collider != null || leftHit.collider != null || rightHit.collider != null;

        // Optional: Ensure we're only grounded on the top of colliders
        if (isGrounded)
        {
            RaycastHit2D hit = centerHit.collider != null ? centerHit :
                               leftHit.collider != null ? leftHit : rightHit;

            // Check if the normal of the surface is pointing up (it's a ground surface)
            if (hit.normal.y < 0.7f)
            {
                isGrounded = false; // Not grounded if the surface is too steep (like a wall)
            }
        }

        // Debugging
        Debug.DrawRay(rayOrigin, Vector2.down * groundRayLength, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(rayOrigin - new Vector2(groundRayOffset.x, 0), Vector2.down * groundRayLength,
                     leftHit.collider != null ? Color.green : Color.red);
        Debug.DrawRay(rayOrigin + new Vector2(groundRayOffset.x, 0), Vector2.down * groundRayLength,
                     rightHit.collider != null ? Color.green : Color.red);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Normal collectibles
        if (collectibles != null && collectibles.Contains(other.gameObject))
        {
            collectibles.Remove(other.gameObject);
            Destroy(other.gameObject);

            if (platformerManager != null)
                platformerManager.AddScore(collectiblePoints);

            PlaySound(normalCollectSound);
        }

        // Special collectible
        if (platformerManager != null && platformerManager.specialCollectible == other.gameObject)
        {
            Destroy(other.gameObject);
            platformerManager.OnSpecialCollectibleGrabbed();
            PlaySound(specialCollectSound);
        }
    }

    private void PlaySound(AudioSource source)
    {
        if (source != null && source.clip != null)
            source.PlayOneShot(source.clip);
    }
}
