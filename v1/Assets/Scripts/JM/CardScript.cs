using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardScript : MonoBehaviour
{
    public Image belowImage;      // Assign in Inspector
    public TMP_Text belowText;    // Assign in Inspector
    public Button coverButton;    // Assign in Inspector

    [HideInInspector] public int cardId; // Set by manager
    [HideInInspector] public bool isImageCard; // true=image, false=text

    private MemoryGameManager gameManager;

    void Start()
    {
        coverButton.onClick.AddListener(OnCardClicked);
        gameManager = FindObjectOfType<MemoryGameManager>();
    }

    public void Setup(Sprite image, string text, bool showImage)
    {
        isImageCard = showImage;
        belowImage.gameObject.SetActive(showImage);
        belowText.gameObject.SetActive(!showImage);

        if (showImage)
        {
            belowImage.sprite = image;
        }
        else
        {
            belowText.text = text;
        }
    }

    public void OnCardClicked()
    {
        if (gameManager != null && gameManager.CanReveal())
        {
            coverButton.gameObject.SetActive(false);
            gameManager.OnCardRevealed(this);
        }
    }
    public void Cover()
    {
        coverButton.gameObject.SetActive(true);
    }

    public void Reveal()
    {
        coverButton.gameObject.SetActive(false);
    }
}
