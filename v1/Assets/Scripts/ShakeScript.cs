using UnityEngine;
using System.Collections;

public class ShakeScript : MonoBehaviour
{
    public float shakeDuration = 0.3f;    // Duration of each shake
    public float shakeMagnitude = 10f;    // How far it shakes (in UI units)
    public float shakeInterval = 5f;      // Time between shakes

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shakeInterval);
            yield return StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            rectTransform.anchoredPosition = originalPosition + new Vector2(offsetX, offsetY);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = originalPosition;
    }
}
