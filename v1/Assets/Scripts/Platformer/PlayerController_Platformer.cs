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

    [Header("Collectibles")]
    public List<GameObject> collectibles; // Assign in Inspector
    public int collectiblePoints = 1;

    private Rigidbody2D rb;
    public Animator animator;
    private bool isGrounded;
    private bool facingRight = true;
    private PlatformerManager platformerManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        platformerManager = FindObjectOfType<PlatformerManager>();
    }

    void Update()
    {
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

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
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
        }

        // Special collectible
        if (platformerManager != null && platformerManager.specialCollectible == other.gameObject)
        {
            Destroy(other.gameObject);
            platformerManager.OnSpecialCollectibleGrabbed();
        }
    }
}