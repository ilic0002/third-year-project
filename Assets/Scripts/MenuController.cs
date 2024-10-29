using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public void LoadPuzzleMode()
    {
        SceneManager.LoadScene("Puzzles"); // Replace with your actual scene name
    }

    public void LoadMathEquations()
    {
        SceneManager.LoadScene("MathEquations"); // Replace with your actual scene name
    }

    public void LoadPatternRecognition()
    {
        SceneManager.LoadScene("PatternRecognitions"); // Replace with your actual scene name
    }
}
