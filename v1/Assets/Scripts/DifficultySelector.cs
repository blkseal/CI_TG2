using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    public static Difficulty selectedDifficulty;

    [Header("Scene to Load")]
    public string sceneName = "GameScene";

    public void SetDifficulty(int level)
    {
        selectedDifficulty = (Difficulty)level;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}