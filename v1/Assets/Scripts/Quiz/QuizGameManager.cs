using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizGameManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers = new string[4];
        public int correctAnswerIndex;
        public Sprite questionImage; // Add image field
    }

    public List<Question> easyQuestions;
    public List<Question> mediumQuestions;
    public List<Question> hardQuestions;

    public TMP_Text questionText;
    public Image questionImageDisplay; // Add reference to image UI element
    public Button[] answerButtons;
    public TMP_Text timerText;

    private List<Question> currentQuestions;
    private int currentQuestionIndex = 0;
    public static int score = 0;
    private float timer = 0f;
    private float timePerQuestion = 30f;

    private Color[] defaultButtonColors = new Color[4];

    private int pointsPerDifficulty
    {
        get
        {
            switch (DifficultySelector.selectedDifficulty)
            {
                case DifficultySelector.Difficulty.Easy: return 10;
                case DifficultySelector.Difficulty.Medium: return 20;
                case DifficultySelector.Difficulty.Hard: return 30;
                default: return 10;
            }
        }
    }

    void Start()
    {
        // Store default button image colors
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                defaultButtonColors[i] = buttonImage.color;
            }
        }

        switch (DifficultySelector.selectedDifficulty)
        {
            case DifficultySelector.Difficulty.Easy:
                currentQuestions = new List<Question>(easyQuestions);
                break;
            case DifficultySelector.Difficulty.Medium:
                currentQuestions = new List<Question>(mediumQuestions);
                break;
            case DifficultySelector.Difficulty.Hard:
                currentQuestions = new List<Question>(hardQuestions);
                break;
        }

        currentQuestionIndex = 0;
        score = 0;
        ShowQuestion();
    }

    void Update()
    {
        timer += Time.deltaTime;
        float timeLeft = Mathf.Max(0, timePerQuestion - timer);
        timerText.text = $"Tempo: {timeLeft:F1}s";

        if (timeLeft <= 0)
        {
            StartCoroutine(ShowAnswerAndNext(false, -1));
        }
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= currentQuestions.Count)
        {
            EndQuiz();
            return;
        }

        timer = 0f;
        var q = currentQuestions[currentQuestionIndex];
        questionText.text = q.questionText;

        // Set question image
        if (questionImageDisplay != null)
        {
            if (q.questionImage != null)
            {
                questionImageDisplay.sprite = q.questionImage;
                questionImageDisplay.gameObject.SetActive(true);
            }
            else
            {
                questionImageDisplay.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(true);
            answerButtons[i].interactable = true;

            // Reset button image color
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            if (buttonImage != null && i < defaultButtonColors.Length)
            {
                buttonImage.color = defaultButtonColors[i];
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < answerButtons.Length && i < q.answers.Length)
            {
                var tmpText = answerButtons[i].GetComponentInChildren<TMP_Text>();
                if (tmpText != null)
                    tmpText.text = q.answers[i];
                int index = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            }
        }
    }

    void OnAnswerSelected(int index)
    {
        bool correct = index == currentQuestions[currentQuestionIndex].correctAnswerIndex;
        StartCoroutine(ShowAnswerAndNext(correct, index));
    }

    IEnumerator ShowAnswerAndNext(bool correct, int selectedIndex)
    {
        // Disable all buttons to prevent further clicks
        foreach (var btn in answerButtons)
            btn.interactable = false;

        int correctIndex = currentQuestions[currentQuestionIndex].correctAnswerIndex;

        // Always highlight the correct answer in green
        SetImageColor(answerButtons[correctIndex], new Color(0.4f, 0.8f, 0.4f)); // Soft green

        // If wrong answer selected, highlight it in red
        if (!correct && selectedIndex >= 0 && selectedIndex < answerButtons.Length)
            SetImageColor(answerButtons[selectedIndex], new Color(0.8f, 0.4f, 0.4f)); // Soft red

        float timeLeft = Mathf.Max(0, timePerQuestion - timer);
        if (correct)
        {
            int points = Mathf.Max(1, Mathf.RoundToInt(pointsPerDifficulty * (timeLeft / timePerQuestion)));
            score += points;
        }

        yield return new WaitForSeconds(1.2f);

        currentQuestionIndex++;
        ShowQuestion();
    }

    void SetImageColor(Button btn, Color color)
    {
        Image buttonImage = btn.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }
    }

    void EndQuiz()
    {
        SceneManager.LoadScene("QuizFinalScene");
    }
}
