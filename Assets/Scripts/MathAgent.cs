using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using TMPro;


public class MathAgent : Agent
{
    // Reference to the MathEquationManager to get the current equation and correct answer
    public MathEquationManager equationManager;

    public TextMeshProUGUI equationText;

    private int correctAnswer;
    private string currentEquation;
    private bool isEquationTrue;

    public override void OnEpisodeBegin()
    {
        // Reset the environment and get the current equation
        equationManager.GenerateNewEquation(); // This is called from MathEquationManager
        currentEquation = equationManager.currentEquation; // Set the current equation
        correctAnswer = equationManager.correctAnswer; // Set the correct answer
        isEquationTrue = equationManager.isEquationTrue; // True/False of the equation
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe the math problem: operands, operator, and displayed answer
        string[] equationParts = currentEquation.Split(' ');
        int a = int.Parse(equationParts[0]);
        int b = int.Parse(equationParts[2]);

        // Add the operands and the displayed answer as observations
        sensor.AddObservation(a);
        sensor.AddObservation(b);
        sensor.AddObservation(int.Parse(equationParts[4])); // The displayed answer
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Action: either choose True/False or the numeric answer
        float action = actions.ContinuousActions[0]; // Action taken by the agent
        float reward = 0f;

        // If the action is a numeric answer, check if it matches the correct answer
        if (Mathf.Approximately(action, correctAnswer))
        {
            reward = 1f; // Correct answer
        }
        else
        {
            reward = -1f; // Incorrect answer
        }

        // Give reward for the action (correct or incorrect)
        SetReward(reward);

        // End the episode after checking the answer
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // This function allows manual control for testing
        actionsOut.ContinuousActions.Array[0] = correctAnswer; // Set the correct answer
    }
}
