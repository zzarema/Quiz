using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feedbackText;
    public Button backButton;             
    public GameObject finishCanvas;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip completionSound;

    private AudioSource audioSource;
    private int currentQuestionIndex = 0;
    private int score = 0;

    private Question[] questions = new Question[]
    {
        new Question("What is the capital of France?", "Paris", "London", "Berlin", "Rome", 0),
        new Question("Which river is the longest in the world?", "Amazon", "Nile", "Mississippi", "Yangtze", 1),
        new Question("Which is the largest country in Africa?", "Algeria", "Nigeria", "South Africa", "Egypt", 0),
        new Question("Which country is known as the Land of the Rising Sun?", "China", "Japan", "South Korea", "Thailand", 1),
        new Question("Which country has the largest population?", "India", "United States", "China", "Indonesia", 2),
        new Question("Which country is the largest by area?", "Canada", "United States", "Russia", "China", 2),
        new Question("What is the capital of Kazakhstan?", "Astana", "Almaty", "Bishkek", "Tashkent", 0),
        new Question("Which continent is known as the Dark Continent?", "Asia", "Africa", "South America", "Australia", 1),
        new Question("Which country is famous for its pyramids?", "Egypt", "Greece", "Mexico", "Italy", 0),
        new Question("Which country is both in Europe and Asia?", "Turkey", "Russia", "Kazakhstan", "Egypt", 1)
    };

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        backButton.gameObject.SetActive(false);
        finishCanvas.SetActive(false);
        LoadQuestion();
    }
    public void LoadQuestion()
    {
        if (currentQuestionIndex < questions.Length)  // ”бедитесь, что индекс в пределах массива
        {
            Question currentQuestion = questions[currentQuestionIndex];

            questionText.text = currentQuestion.question;
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
                int buttonIndex = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(buttonIndex));
            }

            questionText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true);
            feedbackText.gameObject.SetActive(true);

            scoreText.text = "Score: " + score;
            feedbackText.text = "";
            feedbackText.color = Color.white;

            backButton.gameObject.SetActive(false);
            finishCanvas.SetActive(false);
        }
        else
        {
            ShowFinalScreen();
        }
    }

    public void CheckAnswer(int selectedAnswer)
    {
        // ѕроверка, чтобы не выйти за пределы массива
        if (currentQuestionIndex >= questions.Length) return;

        if (selectedAnswer == questions[currentQuestionIndex].correctAnswer)
        {
            score++;
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
            AudioSource.PlayClipAtPoint(correctSound, Camera.main.transform.position);
        }
        else
        {
            feedbackText.text = "Incorrect!";
            feedbackText.color = Color.red;
            AudioSource.PlayClipAtPoint(incorrectSound, Camera.main.transform.position);
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


    private void ShowFinalScreen()
    {
        foreach (Button button in answerButtons)
        {
            button.gameObject.SetActive(false);
        }
        questionText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);

        AudioSource.PlayClipAtPoint(completionSound, Camera.main.transform.position);

        finishCanvas.SetActive(true);

        TextMeshProUGUI finalText = finishCanvas.GetComponentInChildren<TextMeshProUGUI>();
        if (finalText != null)
        {
            finalText.text = "Congratulations!\nYou have completed this level!\nYour final score: " + score;
        }

        backButton.gameObject.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
}

public class Question
{
    public string question;
    public string[] answers;
    public int correctAnswer;

    public Question(string question, string answer1, string answer2, string answer3, string answer4, int correctAnswer)
    {
        this.question = question;
        this.answers = new string[] { answer1, answer2, answer3, answer4 };
        this.correctAnswer = correctAnswer;
    }
}
