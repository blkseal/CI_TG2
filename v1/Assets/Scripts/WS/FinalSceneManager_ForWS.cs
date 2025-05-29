using UnityEngine;
using TMPro;

public class FinalSceneManager_ForWS : MonoBehaviour
{
    public TMP_Text scoreText;       // Assign in Inspector
    public TMP_Text highScoreText;   // Assign in Inspector
    public TMP_Text topSpeedText;    // Assign in Inspector

    private const string HighScoreKey = "WS_HighScore";
    private const string LastScoreKey = "WS_LastScore";
    private const string TopSpeedKey = "WS_TopSpeed";

    void Start()
    {
        // Get the player's score and top speed from PlayerPrefs
        int score = PlayerPrefs.GetInt(LastScoreKey, 0);
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        float topSpeed = PlayerPrefs.GetFloat(TopSpeedKey, 800f); // Default to base speed

        // Update high score if needed
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        // Display the scores
        scoreText.text = $"Pontuação: {score}";
        highScoreText.text = $"Melhor pontuação: {highScore}";

        // Display top speed
        topSpeedText.text = $"Velocidade maior: {topSpeed:F1}";
    }
}

