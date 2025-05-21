using UnityEngine;
using TMPro;

public class FinalSceneManager_ForRA : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    void Start()
    {
        int score = PlayerPrefs.GetInt("Score", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Update high score if the new score is higher
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Display scores
        if (scoreText != null)
            scoreText.text = $"Pontuação: {score}";

        if (highScoreText != null)
            highScoreText.text = $"Melhor Pontuação: {highScore}";
    }
}