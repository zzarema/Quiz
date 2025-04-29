using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level3Manager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInput;  // Используем TMP_InputField
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feedbackText;
    public Button submitButton;  // Кнопка для отправки ответа
    public Button backButton;
    public GameObject finishCanvas;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip completionSound;

    private AudioSource audioSource;
    private int currentQuestionIndex = 0;
    private int score = 0;

    private WordQuestion[] questions = new WordQuestion[]
    {
        new WordQuestion("A_R_I_C_A", "AFRICA"),
        new WordQuestion("E_R_P_E", "EUROPE"),
        new WordQuestion("C_N_D_A", "CANADA"),
        new WordQuestion("A_I_", "ASIA"),
        new WordQuestion("A_L__T_C", "ATLANTIC"),
        new WordQuestion("I__IA", "INDIA"),
        new WordQuestion("A__T__L_A", "AUSTRALIA"),
        new WordQuestion("B__Z__", "BRAZIL"),
        new WordQuestion("G__M__Y", "GERMANY"),
        new WordQuestion("K_______N", "KAZAKHSTAN")
    };

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        submitButton.onClick.AddListener(CheckAnswer);  // Подключаем кнопку к методу CheckAnswer
        backButton.gameObject.SetActive(false);
        finishCanvas.SetActive(false);
        LoadQuestion();
    }

    public void LoadQuestion()
    {
        if (currentQuestionIndex < questions.Length)
        {
            WordQuestion currentQuestion = questions[currentQuestionIndex];

            // Отображаем слово с пробелами для угадывания
            questionText.text = "Fill in the missed places: " + currentQuestion.incompleteWord;

            answerInput.text = "";  // Clear the previous input
            feedbackText.text = "";
            feedbackText.color = Color.white;

            scoreText.text = "Score: " + score;

            backButton.gameObject.SetActive(false);
            finishCanvas.SetActive(false);
        }
        else
        {
            ShowFinalScreen();
        }
    }

    public void CheckAnswer()
    {
        WordQuestion currentQuestion = questions[currentQuestionIndex];

        if (answerInput.text.ToUpper() == currentQuestion.correctWord.ToUpper())
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

public class WordQuestion
{
    public string incompleteWord;
    public string correctWord;

    public WordQuestion(string incompleteWord, string correctWord)
    {
        this.incompleteWord = incompleteWord;
        this.correctWord = correctWord;
    }
}
