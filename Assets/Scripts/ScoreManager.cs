using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // Reference to the UI text
    private int score = 0;

    void Start()
    {
        // Check if the scoreText is assigned
        if (scoreText == null)
        {
            Debug.LogError("ScoreText is not assigned in the Inspector.");
            return;
        }

        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        // Ensure that the score is properly updated
        score += amount;

        // Log the updated score for debugging purposes
        Debug.Log("Score added: " + amount + ", New score: " + score);

        // Update the UI with the new score
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        // Check if scoreText is assigned and update the UI text
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;  // Update the score UI
        }
        else
        {
            Debug.LogError("ScoreText reference is missing!");
        }
    }
}