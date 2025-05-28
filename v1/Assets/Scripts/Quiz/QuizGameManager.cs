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
    }

    public List<Question> easyQuestions;
    public List<Question> mediumQuestions;
    public List<Question> hardQuestions;

    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text timerText;

    private List<Question> currentQuestions;
    private int currentQuestionIndex = 0;
    public static int score = 0; // Make score static to access in final scene
    private float timer = 0f;
    private float timePerQuestion = 15f; // seconds per question

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
        // Select questions based on difficulty
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
        timerText.text = $"Time: {timeLeft:F1}s";

        if (timeLeft <= 0)
        {
            NextQuestion(false);
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

        // Reactivate buttons in case they were deactivated
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < 4; i++)
        {
            if (i < answerButtons.Length && i < q.answers.Length)
            {
                var tmpText = answerButtons[i].GetComponentInChildren<TMP_Text>();
                if (tmpText != null)
                    tmpText.text = q.answers[i];
                int index = i; // capture for lambda
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            }
        }
    }

    void OnAnswerSelected(int index)
    {
        bool correct = index == currentQuestions[currentQuestionIndex].correctAnswerIndex;
        NextQuestion(correct);
    }

    void NextQuestion(bool correct)
    {
        float timeLeft = Mathf.Max(0, timePerQuestion - timer);
        if (correct)
        {
            // Points decrease as time passes, minimum 1 point per question
            int points = Mathf.Max(1, Mathf.RoundToInt(pointsPerDifficulty * (timeLeft / timePerQuestion)));
            score += points;
        }
        currentQuestionIndex++;
        ShowQuestion();
    }

    void EndQuiz()
    {
        // Optionally, you can show a "Quiz Complete!" message briefly here
        // before loading the final scene.
        SceneManager.LoadScene("QuizFinalScene");
    }
}
