using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatternRecognitionGame : MonoBehaviour
{
    public Image originalPatternDisplay;  // Display for the original pattern
    public Button[] optionButtons;        // Buttons for multiple choice options
    public Sprite[] patternSprites;       // List of pattern images for choices
    public TextMeshProUGUI timerText;     // Optional: Timer text for countdown
    public TextMeshProUGUI feedbackText;  // New TextMeshPro UI for feedback (Correct or Wrong)
    public float displayTime = 5.0f;      // Initial time for displaying the pattern
    public int playerCorrectCount = 0;    // Track consecutive correct answers
    private float timeLeft;               // Timer for how long to display the pattern

    private Sprite[] currentOptions;      // Track the current options for the round
    private bool isRoundActive = false;   // Track if the round is active (to prevent multiple inputs)
    private bool hasAnswered = false;     // Track if the player has already answered the current round

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
        // Only shuffle and assign options once per round
        if (currentOptions == null || currentOptions.Length != optionButtons.Length)
        {
            currentOptions = new Sprite[optionButtons.Length];

            // Shuffle and assign unique options to each button
            for (int i = 0; i < optionButtons.Length; i++)
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, patternSprites.Length);
                }
                while (ArrayContains(currentOptions, patternSprites[randomIndex])); // Ensure no duplicates
                currentOptions[i] = patternSprites[randomIndex];
            }
        }

        // Assign the options to the buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].GetComponent<Image>().sprite = currentOptions[i];

            // Remove any existing listeners to prevent multiple invocations
            optionButtons[i].onClick.RemoveAllListeners();

            // Add listener for button clicks (ensures only one click action per round)
            int buttonIndex = i; // Local copy for lambda
            optionButtons[i].onClick.AddListener(() => OnChoiceSelected(buttonIndex));
        }

        // Disable the original pattern display and enable buttons for selection
        originalPatternDisplay.gameObject.SetActive(false);
        SetButtonsInteractable(true); // Enable buttons for selection
        isRoundActive = true; // Mark the round as active
        hasAnswered = false;  // Reset the answer flag for the new round
    }

    // Method to check if an array already contains a specific sprite
    bool ArrayContains(Sprite[] array, Sprite sprite)
    {
        foreach (Sprite s in array)
        {
            if (s == sprite) return true;
        }
        return false;
    }

    // Method to handle the button click event
    void OnChoiceSelected(int buttonIndex)
    {
        if (!isRoundActive || hasAnswered) return; // If the round is not active or already answered, do nothing

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
        SetButtonsInteractable(false);
        hasAnswered = true; // Mark that the player has answered
    }

    // Handle correct choice
    void OnCorrectChoice()
    {
        Debug.Log("Correct!");
        feedbackText.text = "Correct!";   // Show feedback to the player
        playerCorrectCount++;             // Increment correct count
        AdjustDisplayTime();              // Adjust difficulty

        // Give feedback that the player was correct, then move to the next round
        Invoke("NextRound", 1f);  // Delay before moving to the next round
    }

    // Handle wrong choice
    void OnWrongChoice()
    {
        Debug.Log("Try again!");
        feedbackText.text = "Wrong!";     // Show feedback to the player
        playerCorrectCount = 0;           // Reset correct count if wrong choice
        AdjustDisplayTime();              // Adjust difficulty

        // Show the pattern again for the player to try again
        Invoke("ShowPattern", 1f); // Delay before showing the pattern again
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
        // Disable the buttons after a choice is made
        SetButtonsInteractable(true);
    }

    // Method to enable or disable buttons
    void SetButtonsInteractable(bool interactable)
    {
        foreach (Button btn in optionButtons)
        {
            btn.interactable = interactable;
        }
    }

    // Proceed to the next round (or retry in case of wrong answer)
    void NextRound()
    {
        ShowPattern();  // Show a new pattern for the next round
        ResetButtons(); // Reset the buttons for the next round
        feedbackText.text = ""; // Clear the feedback text
    }
}
