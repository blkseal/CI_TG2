using UnityEngine;

public class BackArrowButton : MonoBehaviour
{
    public void GoBack()
    {
        if (SceneTracker.Instance != null)
            SceneTracker.Instance.GoBack();
    }
}
