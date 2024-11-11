using UnityEngine;
using UnityEngine.UI;
using TMPro; // if you're using TextMeshPro for text UI

public class PatternRecognitionGame : MonoBehaviour
{
    public Image originalPatternDisplay;   // Display for the original pattern
    public Button[] optionButtons;         // Buttons for multiple choice options
    public Sprite[] patternSprites;        // List of pattern images for choices
    public TextMeshProUGUI timerText;      // Optional: Timer text for countdown
    public float displayTime = 5.0f;       // Initial time for displaying the pattern
    public int playerCorrectCount = 0;     // Track consecutive correct answers
    private float timeLeft;                // Timer for how long to display the pattern

    void Start()
    {
        // Initialize the timer
        timeLeft = displayTime;
        // Start the game by showing the first pattern
        ShowPattern();
    }

    void Update()
    {
        // Handle countdown timer for pattern display (if using timer)
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Time Left: " + Mathf.Ceil(timeLeft).ToString("0");
        }
        else
        {
            // Time is up, hide the original pattern and show options
            originalPatternDisplay.gameObject.SetActive(false);
            ShowChoices();
        }
    }

    // Method to show the pattern to the player
    void ShowPattern()
    {
        originalPatternDisplay.gameObject.SetActive(true);

        // Choose a random pattern to show
        int randomIndex = Random.Range(0, patternSprites.Length);
        originalPatternDisplay.sprite = patternSprites[randomIndex];

        // Reset the timer when showing the pattern
        timeLeft = displayTime;
    }

    // Method to show the multiple choice buttons
    void ShowChoices()
    {
        // Randomly shuffle and assign patterns to buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int randomIndex = Random.Range(0, patternSprites.Length);
            optionButtons[i].GetComponent<Image>().sprite = patternSprites[randomIndex];
            
            // Add listener for button clicks
            int buttonIndex = i; // Local copy for lambda
            optionButtons[i].onClick.AddListener(() => OnChoiceSelected(buttonIndex));
        }
    }

    // Method to handle the button click event
    void OnChoiceSelected(int buttonIndex)
    {
        // Get the image of the button clicked
        Image buttonImage = optionButtons[buttonIndex].GetComponent<Image>();
        
        // Compare the selected pattern with the original one
        if (buttonImage.sprite == originalPatternDisplay.sprite)
        {
            OnCorrectChoice();
        }
        else
        {
            OnWrongChoice();
        }

        // Disable buttons after selection (optional)
        foreach (Button btn in optionButtons)
        {
            btn.interactable = false;
        }
    }

    // Handle correct choice
    void OnCorrectChoice()
    {
        Debug.Log("Correct!");
        playerCorrectCount++; // Increment correct count
        AdjustDisplayTime();  // Adjust difficulty

        // Restart the game or load the next round
        ShowPattern();
        ResetButtons();
    }

    // Handle wrong choice
    void OnWrongChoice()
    {
        Debug.Log("Try again!");
        playerCorrectCount = 0; // Reset correct count if wrong choice
        AdjustDisplayTime();    // Adjust difficulty

        // Show the pattern again for the player to try again
        ShowPattern();
        ResetButtons();
    }

    // Method to adjust the display time based on player performance
    void AdjustDisplayTime()
    {
        if (playerCorrectCount >= 3) // Increase difficulty
        {
            displayTime = Mathf.Max(1.5f, displayTime - 0.5f); // Decrease time but not below 1.5 seconds
        }
        else if (playerCorrectCount < 1) // Ease difficulty
        {
            displayTime = Mathf.Min(5.0f, displayTime + 0.5f); // Increase time but not above 5 seconds
        }
    }

    // Reset the buttons (make them interactable again for the next round)
    void ResetButtons()
    {
        foreach (Button btn in optionButtons)
        {
            btn.interactable = true;
        }
    }
}
