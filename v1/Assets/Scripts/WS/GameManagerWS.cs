using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerWS : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timerText;
    private int score = 0;
    private float timer = 30f;
    private bool gameActive = true;

    // Track the player's top speed
    private PlayerController player;
    private float topSpeed = 0f;

    private int scoreMultiplier = 1;

    // AudioSources for feedback
    public AudioSource badHitAudioSource;
    public AudioSource goodHitAudioSource;
    public AudioSource shootingAudioSource;

    public void SetScoreMultiplier(int multiplier)
    {
        scoreMultiplier = multiplier;
    }

    public void AddScore(int value)
    {
        if (!gameActive) return;
        score += value * scoreMultiplier;
        if (score < 0) score = 0; // Nunca deixa o score ir abaixo de zero
        scoreText.text = $"Score: {score}";
    }

    public void PlayBadHitSound()
    {
        if (badHitAudioSource != null)
            badHitAudioSource.Play();
    }

    public void PlayGoodHitSound()
    {
        if (goodHitAudioSource != null)
            goodHitAudioSource.Play();
    }

    public void PlayShootingSound()
    {
        if (shootingAudioSource != null)
            shootingAudioSource.Play();
    }

    void Start()
    {
        // Find player - wait a moment to ensure it's spawned by Spawner
        Invoke(nameof(FindPlayer), 0.5f);
    }

    void FindPlayer()
    {
        player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
        }
    }

    void Update()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        timerText.text = $"Time: {Mathf.CeilToInt(timer)}";

        // Track top speed if player exists
        if (player != null && player.moveSpeed > topSpeed)
        {
            topSpeed = player.moveSpeed;
        }

        if (timer <= 0f)
        {
            timer = 0f;
            timerText.text = "Time: 0";
            gameActive = false;
            EndGame();
        }
    }

    void EndGame()
    {
        // Save game data to PlayerPrefs
        PlayerPrefs.SetInt("WS_LastScore", score);
        PlayerPrefs.SetFloat("WS_TopSpeed", topSpeed);
        PlayerPrefs.Save();

        // Wait a short moment before transitioning (optional)
        Invoke(nameof(GoToFinalScene), 1.5f);
    }

    void GoToFinalScene()
    {
        SceneManager.LoadScene("WSFinalScene");
    }
}