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

    private string currentEquation;
    private int correctAnswer;
    private bool isEquationTrue;

    void Start()
    {
        GenerateNewEquation();
        trueButton.onClick.AddListener(() => CheckTrueFalseAnswer(true));
        falseButton.onClick.AddListener(() => CheckTrueFalseAnswer(false));
        submitButton.onClick.AddListener(CheckInputAnswer);
    }

    void GenerateNewEquation()
    {
        // Example: Generate a simple equation
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        correctAnswer = a + b;

        // Randomly decide if the equation is true or false
        isEquationTrue = Random.value > 0.5f;
        int displayedAnswer = isEquationTrue ? correctAnswer : correctAnswer + Random.Range(-2, 2);

        currentEquation = $"{a} + {b} = {displayedAnswer}";
        equationText.text = currentEquation;
        feedbackText.text = "";
        answerInputField.text = "";
    }

    void CheckTrueFalseAnswer(bool answer)
    {
        if (answer == isEquationTrue)
            feedbackText.text = "Correct!";
        else
            feedbackText.text = "Incorrect!";
    }

    void CheckInputAnswer()
    {
        if (int.TryParse(answerInputField.text, out int userAnswer))
        {
            if (userAnswer == correctAnswer)
                feedbackText.text = "Correct!";
            else
                feedbackText.text = "Incorrect!";
        }
        else
        {
            feedbackText.text = "Please enter a valid number!";
        }
    }
}
