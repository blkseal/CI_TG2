using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FallingFruit : MonoBehaviour
{
    public float fallSpeed = 400f; // Pixels per second
    public float startDelay = 0f;  // Optional delay before falling

    private RectTransform rectTransform;
    private float canvasHeight;
    private Vector2 startPosition;
    private bool isFalling = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;

        // Get the height of the parent Canvas
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
            canvasHeight = canvasRect.rect.height;
        }
        else
        {
            Debug.LogError("FallingFruit: No parent Canvas found.");
        }

        StartCoroutine(StartFallingAfterDelay());
    }

    IEnumerator StartFallingAfterDelay()
    {
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);
        isFalling = true;
    }

    void Update()
    {
        if (!isFalling)
            return;

        // Move down
        rectTransform.anchoredPosition -= new Vector2(0, fallSpeed * Time.deltaTime);

        // If the image is below the bottom of the canvas, respawn at the top
        float halfHeight = rectTransform.rect.height / 2f;
        float bottomY = -canvasHeight / 2f - halfHeight;
        if (rectTransform.anchoredPosition.y < bottomY)
        {
            rectTransform.anchoredPosition = startPosition;
            isFalling = false;
            StartCoroutine(StartFallingAfterDelay());
        }
    }
}
