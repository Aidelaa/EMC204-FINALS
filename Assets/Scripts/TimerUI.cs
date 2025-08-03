using UnityEngine;
using TMPro;  // Import TextMeshPro namespace

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // Reference to the TextMeshProUGUI component
    public float startTime = 60f;     // Start time for the countdown (5 minutes in seconds)
    private float timeRemaining;       // Time left in the countdown
    private bool isCountingDown = false; // Whether the timer is running or not

    // Reference to all spawners (this can be any spawner, not just enemy spawners)
    public MonoBehaviour[] spawners;  // Assign your spawners in the Inspector

    void Start()
    {
        // Initialize the timer with 5 minutes
        timeRemaining = startTime;
        timerText.text = FormatTime(timeRemaining);  // Display initial time

        // Start the countdown automatically as the game starts
        StartCountdown();
    }

    void Update()
    {
        // If the timer is counting down, update the time remaining
        if (isCountingDown)
        {
            timeRemaining -= Time.deltaTime;  // Subtract time each frame
            timerText.text = FormatTime(timeRemaining);  // Update UI with the remaining time

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isCountingDown = false;
                timerText.text = "Time's Up!";  // Display 'Time's Up!' when the timer reaches 0
                HandleTimeUp();  // Call the function to stop everything
                Debug.Log("Timer ended!");
            }
        }
    }

    // This method starts the countdown
    public void StartCountdown()
    {
        isCountingDown = true;
        Debug.Log("Countdown Started!");
    }

    // Optionally, reset the timer to the starting value
    public void ResetTimer()
    {
        timeRemaining = startTime;  // Reset timer to 5 minutes
        timerText.text = FormatTime(timeRemaining);  // Update UI
        isCountingDown = false;  // Stop the countdown
        Debug.Log("Timer Reset!");
    }

    // Helper function to format the time in minutes and seconds
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);  // Get minutes
        int seconds = Mathf.FloorToInt(time % 60);  // Get remaining seconds
        return string.Format("{0:00}:{1:00}", minutes, seconds);  // Return formatted time
    }

    // This method is called when the timer reaches 0
    private void HandleTimeUp()
    {
        // Disable gameplay mechanics when the timer is up
        DisableGame();

        // Optionally, show a "Game Over" screen or trigger a game over event
        // For example:
        // ShowGameOverScreen();  // Unimplemented method for showing a game over screen
    }

    // This method disables gameplay mechanics when time is up
    private void DisableGame()
    {
        // Disable Player Movement
        var player = GameObject.FindGameObjectWithTag("PlayerHole");
        if (player != null)
        {
            var playerMovement = player.GetComponent<HoleMovement>(); // Assuming HoleMovement script exists
            if (playerMovement != null) playerMovement.enabled = false;  // Disable player movement
        }

        // Disable Enemy Movement (Or any other specific components)
        var enemies = GameObject.FindGameObjectsWithTag("EnemyHole");
        foreach (var enemy in enemies)
        {
            var enemyMovement = enemy.GetComponent<EnemyHole>(); // Assuming EnemyHole script exists
            if (enemyMovement != null) enemyMovement.enabled = false;  // Disable enemy movement
        }

        // Disable all spawners
        foreach (var spawner in spawners)
        {
            spawner.enabled = false;  // Disable spawning (this can stop any spawner that inherits from MonoBehaviour)
        }

        // Stop any other game logic that you want to freeze when time runs out
        // e.g., disable shooting, jumping, or anything else

        // Example: Pause the game (if necessary)
        Time.timeScale = 0f;  // Pauses the game, if you want to stop everything completely
    }

    // Public method to access the remaining time
    public float GetTimeRemaining()
    {
        return timeRemaining;
    }
}
