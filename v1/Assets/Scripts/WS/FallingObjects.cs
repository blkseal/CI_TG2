using UnityEngine;
using System.Collections;

public class FallingItem : MonoBehaviour
{
    public float fallSpeed = 400f;
    public bool isGoodItem = true;
    public bool isPowerUp = false; // New property for power-ups
    public float powerUpDuration = 5f; // How long the speed boost lasts
    public float speedBoost = 200f; // How much to increase speed

    private RectTransform rectTransform;
    private PlayerController player;
    private GameManagerWS gameManager;
    private float collisionDistance = 100f; // Adjust based on your object sizes

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        player = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManagerWS>();

        // If no player found, try again in Update
    }

    void Update()
    {
        // Find player if not found yet
        if (player == null)
            player = FindObjectOfType<PlayerController>();

        // Move down
        rectTransform.anchoredPosition -= new Vector2(0, fallSpeed * Time.deltaTime);

        // Check collision with player using distance
        if (player != null)
        {
            RectTransform playerRect = player.GetComponent<RectTransform>();
            float distanceX = Mathf.Abs(rectTransform.anchoredPosition.x - playerRect.anchoredPosition.x);
            float distanceY = Mathf.Abs(rectTransform.anchoredPosition.y - playerRect.anchoredPosition.y);

            if (distanceX < collisionDistance && distanceY < collisionDistance)
            {
                // Collision happened
                if (isPowerUp)
                {
                    ApplyPowerUp();
                }
                else if (isGoodItem)
                {
                    gameManager.AddScore(1);
                }
                else
                {
                    gameManager.AddScore(-1);
                }

                Destroy(gameObject);
            }
        }

        // Destroy if off screen
        if (rectTransform.anchoredPosition.y < -Screen.height / 2f - 100f)
        {
            Destroy(gameObject);
        }
    }

    private void ApplyPowerUp()
    {
        if (player != null)
        {
            // Add the speed boost to the player
            StartCoroutine(ApplySpeedBoostCoroutine());
        }
    }

    private IEnumerator ApplySpeedBoostCoroutine()
    {
        // Get current speed
        float originalSpeed = player.moveSpeed;

        // Apply speed boost
        player.moveSpeed += speedBoost;

        // Wait for duration
        yield return new WaitForSeconds(powerUpDuration);

        // Reset speed if the player still exists
        if (player != null)
        {
            player.moveSpeed = originalSpeed;
        }
    }
}
