using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        score += value * 100;
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    public int GetSavedScore()
    {
        return PlayerPrefs.GetInt("Score", 0);
    }
}   