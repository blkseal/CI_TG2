using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlatformerManager : MonoBehaviour
{
    [Header("Score UI")]
    public TMP_Text scoreText; // Assign in Inspector (child of player)

    [Header("Collectibles")]
    public GameObject specialCollectible; // Assign in Inspector

    private int score = 0;
    private bool gameEnded = false;

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        if (gameEnded) return;
        score += points;
        UpdateScoreUI();
    }

    public void OnSpecialCollectibleGrabbed()
    {
        if (gameEnded) return;
        gameEnded = true;
        Debug.Log("Game Ended! Final Score: " + score);
        SceneManager.LoadScene("ChoiceScene");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Pontuação: " + score;
    }
}