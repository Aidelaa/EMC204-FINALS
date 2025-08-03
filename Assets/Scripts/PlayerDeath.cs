using UnityEngine;
using TMPro;  // Import TextMeshPro namespace
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    [HideInInspector] public Vector3 initialPosition;  // Store the player's initial position
    [HideInInspector] public Vector3 initialScale;     // Store the player's initial scale

    [Header("Respawn Settings")]
    public float respawnTime = 5f;        // Time to wait before respawning (seconds)
    public TextMeshProUGUI countdownText; // Reference to the UI TextMeshPro component for countdown

    private bool isDead = false;          // Is the player dead or not
    private bool isRespawning = false;    // Is the player currently respawning?
    private float timeRemaining;          // Track the remaining time for the respawn countdown

    void Start()
    {
        // Remember the initial position and scale of the player
        initialPosition = transform.position;
        initialScale = transform.localScale;

        // Initialize the respawn countdown time
        timeRemaining = respawnTime;

        // Hide the countdown UI initially
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);  // Hide countdown text at start
    }

    void Update()
    {
        // If the player is respawning, update the timer
        if (isRespawning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;  // Subtract time each frame
            countdownText.text = $"Respawning in {Mathf.CeilToInt(timeRemaining)}";  // Update countdown UI

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isRespawning = false;
                countdownText.gameObject.SetActive(false);  // Hide countdown when time is up
                Respawn();  // Call the respawn function when time is up
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player was eaten by a larger enemy
        if (isDead || isRespawning) return;

        if (other.CompareTag("EnemyHole") && other.transform.localScale.x > transform.localScale.x)
        {
            StartCoroutine(DieAndRespawn());
        }
    }

    IEnumerator DieAndRespawn()
    {
        // Set flags to indicate the player is dead and respawning
        isDead = true;
        isRespawning = true;

        // Hide the player object while respawning
        gameObject.SetActive(false);  // Hide the player during respawn countdown

        // Show countdown UI
        countdownText.gameObject.SetActive(true);

        // Wait for the respawn time to complete
        while (isRespawning && timeRemaining > 0)
        {
            yield return null;  // Wait for the next frame
        }

        // After respawn time, call the Respawn function to reset the player
        Respawn();
    }

    void Respawn()
    {
        // Reset position & scale when the respawn countdown reaches zero
        transform.position = initialPosition;
        transform.localScale = initialScale;

        // Reactivate the player object
        gameObject.SetActive(true);  // Show the player after respawn

        // Reset flags after respawn
        isDead = false;
        isRespawning = false;

        Debug.Log("Player respawned, continuing...");
    }
}
