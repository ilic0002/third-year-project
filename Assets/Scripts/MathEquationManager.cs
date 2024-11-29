using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MathEquationManager : MonoBehaviour
{
    public TextMeshProUGUI equationText;
    public TMP_InputField answerInputField;
    public TextMeshProUGUI feedbackText;
    public Button trueButton;
    public Button falseButton;
    public Button submitButton;
    public TextMeshProUGUI gameFinishedText;

    public string currentEquation; 
    public int correctAnswer; 
    public bool isEquationTrue; 
    private int correctAnswersCount = 0;
    private const int maxCorrectAnswers = 10;

    void Start()
    {
        gameFinishedText.gameObject.SetActive(false);
        GenerateNewEquation();
        trueButton.onClick.AddListener(() => CheckTrueFalseAnswer(true));
        falseButton.onClick.AddListener(() => CheckTrueFalseAnswer(false));
        submitButton.onClick.AddListener(CheckInputAnswer);
    }

    public void GenerateNewEquation() 
    {
        if (correctAnswersCount >= maxCorrectAnswers)
        {
            EndGame();
            return;
        }

        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);

        // Randomly choose an operation
        string[] operations = { "+", "-", "*", "/" };
        string operation = operations[Random.Range(0, operations.Length)];

        // Calculate the correct answer based on the chosen operation
        switch (operation)
        {
            case "+":
                correctAnswer = a + b;
                break;
            case "-":
                correctAnswer = a - b;
                break;
            case "*":
                correctAnswer = a * b;
                break;
            case "/":
                b = Random.Range(1, 10);
                a = b * Random.Range(1, 10); 
                correctAnswer = a / b;
                break;
        }

        // Randomly decide if the equation is true or false
        isEquationTrue = Random.value > 0.5f;
        int displayedAnswer = correctAnswer;

        if (!isEquationTrue)
        {
            do
            {
                // Generate a new incorrect answer that is not equal to the correct one
                int offset = Random.Range(-3, 4); 
                if (offset == 0) continue; 
                displayedAnswer = correctAnswer + offset;
            } while (displayedAnswer == correctAnswer || (operation == "/" && displayedAnswer <= 0));
        }

        currentEquation = $"{a} {operation} {b} = {displayedAnswer}";
        equationText.text = currentEquation;
        feedbackText.text = "";
        answerInputField.text = "";
    }

    void CheckTrueFalseAnswer(bool answer)
    {
        if (answer == isEquationTrue)
        {
            feedbackText.text = "Correct!";
            correctAnswersCount++;
            if (correctAnswersCount < maxCorrectAnswers)
                Invoke("GenerateNewEquation", 1.5f);
            else
                EndGame();
        }
        else
        {
            feedbackText.text = "Incorrect!";
        }
    }

    void CheckInputAnswer()
    {
        if (int.TryParse(answerInputField.text, out int userAnswer))
        {
            if (userAnswer == correctAnswer)
            {
                feedbackText.text = "Correct!";
                correctAnswersCount++;
                if (correctAnswersCount < maxCorrectAnswers)
                    Invoke("GenerateNewEquation", 1.5f);
                else
                    EndGame();
            }
            else
            {
                feedbackText.text = "Incorrect!";
            }
        }
        else
        {
            feedbackText.text = "Please enter a valid number!";
        }
    }

    void EndGame()
    {
        feedbackText.text = "";
        equationText.text = "";
        gameFinishedText.text = "Game Finished! You answered 10 equations correctly!";
        gameFinishedText.gameObject.SetActive(true);
    }
}
