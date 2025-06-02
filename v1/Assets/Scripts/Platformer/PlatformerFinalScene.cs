using UnityEngine;
using TMPro;

public class PlatformerFinalScene : MonoBehaviour
{
    public TMP_Text scoreText;      // Assign in Inspector
    public TMP_Text highScoreText;  // Assign in Inspector

    private const string HighScoreKey = "PlatformerHighScore";
    private const string LastScoreKey = "PlatformerCurrentScore";

    void Start()
    {
        int score = PlayerPrefs.GetInt(LastScoreKey, 0);
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        scoreText.text = $"Pontuação: {score}";
        highScoreText.text = $"Melhor Pontuação: {highScore}";
    }
}
