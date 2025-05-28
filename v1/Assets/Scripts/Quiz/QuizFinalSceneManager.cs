using UnityEngine;
using TMPro;

public class QuizFinalSceneManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    private const string HighScoreKey = "QuizHighScore";

    void Start()
    {
        int score = QuizGameManager.score;
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }

        scoreText.text = $"Score: {score}";
        highScoreText.text = $"High Score: {highScore}";
    }
}
