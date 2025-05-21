using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeToScene(string sceneName)
    {
        if (SceneTracker.Instance != null)
            SceneTracker.Instance.RecordCurrentScene();
        SceneManager.LoadScene(sceneName);
    }
}
 