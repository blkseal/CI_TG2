using UnityEngine;
using TMPro;

public class FinalSceneManagerJM : MonoBehaviour
{
    public TMP_Text scoreText;      // Assign in Inspector
    public TMP_Text highScoreText;  // Assign in Inspector

    private const string HighScoreKey = "JM_HighScore";
    private const string LastScoreKey = "JM_LastScore";

    void Start()
    {
        int score = PlayerPrefs.GetInt(LastScoreKey, 0);
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
        }

        scoreText.text = $"Score: {score}";
        highScoreText.text = $"High Score: {highScore}";
    }
}
