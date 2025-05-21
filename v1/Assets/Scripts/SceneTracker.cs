using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance { get; private set; }
    private Stack<string> sceneHistory = new Stack<string>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this before loading a new scene
    public void RecordCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        sceneHistory.Push(currentScene);
    }

    // Call this to go back to the previous scene
    public void GoBack()
    {
        if (sceneHistory.Count > 0)
        {
            string previousScene = sceneHistory.Pop();
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("No previous scene to go back to.");
        }
    }

    // Optional: Clear history if needed
    public void ClearHistory()
    {
        sceneHistory.Clear();
    }
}
