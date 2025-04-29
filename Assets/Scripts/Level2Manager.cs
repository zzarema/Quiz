using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level2Manager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;  // ������ ������ ��� �������
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feedbackText;
    public GameObject finishCanvas;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip completionSound;
    public UnityEngine.UI.Image questionImage;  // ���� ��������� UnityEngine.UI.Image

    private int currentQuestionIndex = 0;
    private int score = 0;

    private Level2Question[] questions = new Level2Question[]
    {
        new Level2Question("The Eiffel Tower is located in Paris.", "True", "False", "Images/Paris", 0),
        new Level2Question("The photo shows Statue of Liberty.", "True", "False", "Images/orig", 1),
        new Level2Question("Mount Everest is the highest mountain in the world.", "True", "False", "Images/everest", 0),
        new Level2Question("The photo shows the Amazon River- the longest river in the world.", "True", "False", "Images/kaspi", 1),
        new Level2Question("The Eiffel Tower is a famous landmark in Paris.", "True", "False", "Images/Paris", 0),
        new Level2Question("The Great Wall of China is located in Japan.", "True", "False", "Images/greatwall", 1),
        new Level2Question("The Earth revolves around the Sun.", "True", "False", "Images/earth", 0),
        new Level2Question("The Pacific Ocean is the largest ocean in the world.", "True", "False", "Images/pasific", 0),
        new Level2Question("Venus is the closest planet to the Sun.", "True", "False", "Images/venus", 1),
        new Level2Question("The Great Barrier Reef is located in Australia.", "True", "False", "Images/great", 0)
    };

    private void Start()
    {
        finishCanvas.SetActive(false);
        LoadQuestion();

        // ����������� ������ ��� ������
        answerButtons[0].onClick.AddListener(HandleTrueButton);  // True
        answerButtons[1].onClick.AddListener(HandleFalseButton); // False
    }

    public void LoadQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            Level2Question currentQuestion = questions[currentQuestionIndex];

            questionText.text = currentQuestion.question;
            scoreText.text = "Score: " + score;
            feedbackText.text = "";
            feedbackText.color = Color.white;

            // �������� ����������� ��� ������� (���� ��� ����)
            if (!string.IsNullOrEmpty(currentQuestion.imageName))
            {
                Sprite questionSprite = Resources.Load<Sprite>(currentQuestion.imageName);
                if (questionSprite != null)
                {
                    questionImage.sprite = questionSprite;
                    questionImage.gameObject.SetActive(true);
                }
                else
                {
                    questionImage.gameObject.SetActive(false);
                }
            }

            // ������������� ����� �� ������
            answerButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answerTrue;
            answerButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answerFalse;

            // ���������� ������
            answerButtons[0].gameObject.SetActive(true);
            answerButtons[1].gameObject.SetActive(true);
        }
        else
        {
            ShowFinalScreen();
        }
    }

    // ����� ��� ��������� ������� ������ True
    public void HandleTrueButton()
    {
        CheckAnswer(0);  // 0 �������� ���������� ����� "True"
    }

    // ����� ��� ��������� ������� ������ False
    public void HandleFalseButton()
    {
        CheckAnswer(1);  // 1 �������� ���������� ����� "False"
    }

    // ����� ��� �������� ������
    public void CheckAnswer(int selectedAnswer)
    {
        if (selectedAnswer == questions[currentQuestionIndex].correctAnswer)
        {
            score++;
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;

            // ������������ ����� � ������� AudioClip
            PlaySound(correctSound);
        }
        else
        {
            feedbackText.text = "Incorrect!";
            feedbackText.color = Color.red;

            // ������������ ����� � ������� AudioClip
            PlaySound(incorrectSound);
        }

        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Length)
        {
            Invoke("LoadQuestion", 1f);
        }
        else
        {
            Invoke("ShowFinalScreen", 1f);
        }
    }

    // ����� ��� ������������ �����
    private void PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    private void ShowFinalScreen()
    {
        answerButtons[0].gameObject.SetActive(false);
        answerButtons[1].gameObject.SetActive(false);

        // ������������ ����� ����������
        PlaySound(completionSound);

        finishCanvas.SetActive(true);

        TextMeshProUGUI finalText = finishCanvas.GetComponentInChildren<TextMeshProUGUI>();
        if (finalText != null)
        {
            finalText.text = "Congratulations!\nYou have completed this level!\nYour final score: " + score;
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

public class Level2Question
{
    public string question;
    public string answerTrue;
    public string answerFalse;
    public string imageName;
    public int correctAnswer;  // 0 ��� True, 1 ��� False

    public Level2Question(string question, string answerTrue, string answerFalse, string imageName, int correctAnswer)
    {
        this.question = question;
        this.answerTrue = answerTrue;
        this.answerFalse = answerFalse;
        this.imageName = imageName;
        this.correctAnswer = correctAnswer;
    }
}
