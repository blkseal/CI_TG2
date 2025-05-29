using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 800f;
    public float margin = 100f; // Margin from the edges

    public GameObject bulletPrefab;
    public float bulletSpeed = 800f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private float canvasWidth;

    // Animator reference
    public Animator animator;
    // isShooting parameter name (should match the parameter in your Animator)
    private static readonly int IsShooting = Animator.StringToHash("isShooting");

    // Canvas bounds (assuming 1920x1080, centered at (0,0))
    private const float canvasTop = 1080f / 2f;
    private const float canvasBottom = -1080f / 2f;
    private const float canvasLeft = -1920f / 2f;
    private const float canvasRight = 1920f / 2f;

    // Track last direction for flipping
    private bool facingRight = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
                Debug.LogError("No Canvas found!");
        }
        canvasWidth = 1920f; // Default width for clamping

        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator component not found on PlayerController GameObject.");
    }

    void Start()
    {
        if (bulletPrefab == null)
            Debug.LogError("Bullet prefab not assigned!");
    }

    void Update()
    {
        // Move with keyboard
        float moveInput = Input.GetAxis("Horizontal");
        float move = moveInput * moveSpeed * Time.deltaTime;
        Vector2 pos = rectTransform.anchoredPosition;

        float leftBound = canvasLeft + margin;
        float rightBound = canvasRight - margin;

        pos.x += move;
        pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);

        rectTransform.anchoredPosition = pos;

        // Flip player based on direction
        if (moveInput < 0 && facingRight)
        {
            Flip(false);
        }
        else if (moveInput > 0 && !facingRight)
        {
            Flip(true);
        }

        // Shoot with spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
            // Set isShooting to true when shooting
            if (animator != null)
                animator.SetBool(IsShooting, true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            // Reset isShooting to false when spacebar is released
            if (animator != null)
                animator.SetBool(IsShooting, false);
        }
    }

    private void Flip(bool faceRight)
    {
        facingRight = faceRight;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (faceRight ? 1 : -1);
        transform.localScale = scale;
    }

    public void Shoot()
    {
        if (bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletPrefab, transform.parent);
        RectTransform bulletRect = bullet.GetComponent<RectTransform>();
        bulletRect.anchoredPosition = rectTransform.anchoredPosition;
        StartCoroutine(MoveBullet(bullet));
    }

    IEnumerator MoveBullet(GameObject bullet)
    {
        RectTransform bulletRect = bullet.GetComponent<RectTransform>();
        while (true)
        {
            bulletRect.anchoredPosition += Vector2.up * bulletSpeed * Time.deltaTime;
            Vector2 pos = bulletRect.anchoredPosition;
            if (pos.y > canvasTop || pos.y < canvasBottom || pos.x < canvasLeft || pos.x > canvasRight)
            {
                Destroy(bullet);
                yield break;
            }
            yield return null;
        }
    }
}