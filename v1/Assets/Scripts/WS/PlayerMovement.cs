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

    // Canvas bounds (assuming 1920x1080, centered at (0,0))
    private const float canvasTop = 1080f / 2f;
    private const float canvasBottom = -1080f / 2f;
    private const float canvasLeft = -1920f / 2f;
    private const float canvasRight = 1920f / 2f;

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
    }

    void Start()
    {
        if (bulletPrefab == null)
            Debug.LogError("Bullet prefab not assigned!");
    }

    void Update()
    {
        // Move with keyboard
        float move = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        Vector2 pos = rectTransform.anchoredPosition;

        float leftBound = canvasLeft + margin;
        float rightBound = canvasRight - margin;

        pos.x += move;
        pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);

        rectTransform.anchoredPosition = pos;

        // Shoot with spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
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
