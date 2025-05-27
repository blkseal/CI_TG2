using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 800f;
    public float margin = 100f; // Simple margin from the edges

    private RectTransform rectTransform;
    private Canvas canvas;
    private float canvasWidth;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
                Debug.LogError("No Canvas found!");
        }

        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        Vector2 pos = rectTransform.anchoredPosition;

        // Calculate boundaries with simple margin
        float leftBound = -canvasWidth / 2 + margin;
        float rightBound = canvasWidth / 2 - margin;

        // Apply movement with clamping
        pos.x += move;
        pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);

        rectTransform.anchoredPosition = pos;
    }
}
