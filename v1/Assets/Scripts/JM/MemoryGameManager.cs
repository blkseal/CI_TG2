using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // Needed for GridLayoutGroup
using UnityEngine.SceneManagement; // Needed for scene loading

[System.Serializable]
public class CardPair
{
    public Sprite image;
    public string word;
}


public class MemoryGameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform gridParent;
    public TMP_Text pointsText;
    public TMP_Text timerText;
    public List<CardPair> cardPairs;

    private List<CardScript> cards = new List<CardScript>();
    private CardScript firstRevealed, secondRevealed;
    private bool canReveal = true;

    public float revealDelay = 1f;

    private int numPairs;
    private int points;
    private float startTime;
    private int matchesFound;
    private int pointsPerMatch;

    public bool CanReveal()
    {
        return canReveal;
    }


    void Start()
    {
        // Set up difficulty
        int columns = 0;
        switch (DifficultySelector.selectedDifficulty)
        {
            case DifficultySelector.Difficulty.Easy:
                numPairs = 3;
                pointsPerMatch = 100;
                columns = 3; // 6 cards (3 pairs)
                break;
            case DifficultySelector.Difficulty.Medium:
                numPairs = 4;
                pointsPerMatch = 200;
                columns = 4; // 8 cards (4 pairs)
                break;
            case DifficultySelector.Difficulty.Hard:
                numPairs = 5;
                pointsPerMatch = 300;
                columns = 5; // 10 cards (5 pairs)
                break;
        }

        // Adapt GridLayoutGroup constraint
        var grid = gridParent.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = columns;
        }

        SetupCards();
        points = 0;
        matchesFound = 0;
        startTime = Time.time;
        UpdatePointsUI();
        UpdateTimerUI();
    }

    void Update()
    {
        UpdateTimerUI();
    }

    void SetupCards()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);
        cards.Clear();

        List<(int pairId, bool isImageCard)> cardData = new List<(int, bool)>();
        for (int i = 0; i < numPairs; i++)
        {
            cardData.Add((i, true));
            cardData.Add((i, false));
        }

        for (int i = 0; i < cardData.Count; i++)
        {
            int rnd = Random.Range(i, cardData.Count);
            var temp = cardData[i];
            cardData[i] = cardData[rnd];
            cardData[rnd] = temp;
        }

        for (int i = 0; i < cardData.Count; i++)
        {
            var data = cardData[i];
            GameObject cardObj = Instantiate(cardPrefab, gridParent);
            CardScript card = cardObj.GetComponent<CardScript>();
            card.cardId = data.pairId;
            if (data.isImageCard)
                card.Setup(cardPairs[data.pairId].image, null, true);
            else
                card.Setup(null, cardPairs[data.pairId].word, false);
            card.Cover();
            cards.Add(card);
        }
    }

    public void OnCardRevealed(CardScript card)
    {
        if (!canReveal || card == firstRevealed) return;

        if (firstRevealed == null)
        {
            firstRevealed = card;
        }
        else
        {
            secondRevealed = card;
            canReveal = false;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(revealDelay);

        if (firstRevealed.cardId == secondRevealed.cardId)
        {
            points += pointsPerMatch;
            matchesFound++;
            UpdatePointsUI();

            if (matchesFound == numPairs)
            {
                EndGame();
                yield break; // Stop here, don't reset canReveal
            }
            canReveal = true; // Allow next move immediately after a match
        }
        else
        {
            // Deactivate game for a few seconds on failed pairing
            float failDelay = 1.5f; // You can adjust this value
            yield return new WaitForSeconds(failDelay);

            firstRevealed.Cover();
            secondRevealed.Cover();
            canReveal = true;
        }

        // Always update points UI (even if points didn't change)
        UpdatePointsUI();

        firstRevealed = null;
        secondRevealed = null;
    }


    void EndGame()
    {
        float timeTaken = Time.time - startTime;
        int timePenalty = Mathf.RoundToInt(timeTaken);
        int finalScore = Mathf.Max(points - timePenalty, 0);

        // Move to next scene and save score
        PlayerPrefs.SetInt("JM_LastScore", finalScore);
        SceneManager.LoadScene("JMFinalScene");
    }


    void UpdatePointsUI()
    {
        pointsText.text = $"Points: {points}";
    }

    void UpdateTimerUI()
    {
        float elapsed = Time.time - startTime;
        timerText.text = $"Time: {elapsed:F1}s";
    }
}
