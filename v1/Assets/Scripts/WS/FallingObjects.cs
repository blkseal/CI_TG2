using UnityEngine;
using System.Collections;

public class FallingItem : MonoBehaviour
{
    public float fallSpeed = 400f;
    public bool isGoodItem = true;
    public bool isPowerUp = false; // For speed boost
    public bool isScoreMultiplier = false; // For score multiplier power-up
    public float powerUpDuration = 5f; // Duration for both power-ups
    public float speedBoost = 200f; // For speed boost

    private RectTransform rectTransform;
    private PlayerController player;
    private GameManagerWS gameManager;
    private float collisionDistance = 100f; // Adjust based on your object sizes
    private float bulletCollisionDistance = 60f; // Adjust as needed for bullet size

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        player = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManagerWS>();
    }

    void Update()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();

        rectTransform.anchoredPosition -= new Vector2(0, fallSpeed * Time.deltaTime);

        // --- Player collision (ALWAYS check for all items) ---
        if (player != null)
        {
            RectTransform playerRect = player.GetComponent<RectTransform>();
            float distanceX = Mathf.Abs(rectTransform.anchoredPosition.x - playerRect.anchoredPosition.x);
            float distanceY = Mathf.Abs(rectTransform.anchoredPosition.y - playerRect.anchoredPosition.y);

            if (distanceX < collisionDistance && distanceY < collisionDistance)
            {
                if (isScoreMultiplier)
                {
                    ApplyScoreMultiplier();
                    if (gameManager != null)
                        gameManager.PlayGoodHitSound();
                }
                else if (isPowerUp)
                {
                    ApplySpeedBoost();
                    if (gameManager != null)
                        gameManager.PlayGoodHitSound();
                }
                else if (isGoodItem)
                {
                    gameManager.AddScore(1);
                    if (gameManager != null)
                        gameManager.PlayGoodHitSound();
                }
                else
                {
                    gameManager.AddScore(-1);
                    if (gameManager != null)
                        gameManager.PlayBadHitSound();
                    player.FlashRed();
                }

                Destroy(gameObject);
                return; // Don't check for bullet collision if already collected by player
            }
        }

        // --- Bullet collision (for bad items only) ---
        if (!isPowerUp && !isScoreMultiplier && !isGoodItem)
        {
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets)
            {
                RectTransform bulletRect = bullet.GetComponent<RectTransform>();
                float dist = Vector2.Distance(rectTransform.anchoredPosition, bulletRect.anchoredPosition);
                if (dist < bulletCollisionDistance)
                {
                    Destroy(bullet);
                    gameManager.AddScore(1); // Add points for destroying bad item with bullet
                    Destroy(gameObject);
                    return;
                }
            }
        }

        // Destroy if off screen
        if (rectTransform.anchoredPosition.y < -Screen.height / 2f - 100f)
        {
            Destroy(gameObject);
        }
    }

    private void ApplySpeedBoost()
    {
        if (player != null)
        {
            StartCoroutine(ApplySpeedBoostCoroutine());
        }
    }

    private IEnumerator ApplySpeedBoostCoroutine()
    {
        float originalSpeed = player.moveSpeed;
        player.moveSpeed += speedBoost;
        yield return new WaitForSeconds(powerUpDuration);
        if (player != null)
        {
            player.moveSpeed = originalSpeed;
        }
    }

    private void ApplyScoreMultiplier()
    {
        if (gameManager != null)
        {
            StartCoroutine(ApplyScoreMultiplierCoroutine());
        }
    }

    private IEnumerator ApplyScoreMultiplierCoroutine()
    {
        gameManager.SetScoreMultiplier(2); // Set multiplier to 2x
        yield return new WaitForSeconds(powerUpDuration);
        gameManager.SetScoreMultiplier(1); // Reset to normal
    }
}