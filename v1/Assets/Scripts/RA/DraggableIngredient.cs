using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableIngredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 startAnchoredPosition;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvas;

    public string correctCategory; // Ex: "Fruta"

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startAnchoredPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false; // allows raycast to target zone
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject target = eventData.pointerEnter;
        if (target != null && target.CompareTag("Target"))
        {
            TargetZone zone = target.GetComponent<TargetZone>();
            if (zone != null && zone.category == correctCategory)
            {
                // Correct drop!
                ScoreManager.instance.AddScore(1);
                // Make invisible and prevent further dragging
                gameObject.SetActive(false);

                // Check if this is the last draggable ingredient
                if (FindObjectsOfType<DraggableIngredient>().Length == 1)
                {
                    SceneManager.LoadScene("RAFinalScene");
                }
                return;
            }
        }

        // Incorrect drop → reset position & score
        ScoreManager.instance.AddScore(-1);
        rectTransform.anchoredPosition = startAnchoredPosition;
    }
}