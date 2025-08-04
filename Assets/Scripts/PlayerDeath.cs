using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    [HideInInspector] public Vector3 initialPosition;
    [HideInInspector] public Vector3 initialScale;

    [Header("Respawn Settings")]
    public float respawnTime = 5f;
    public TextMeshProUGUI countdownText;

    [Header("Player Components")]
    public GameObject playerVisuals; // Drag your player mesh or visuals here in Inspector

    private bool isDead = false;
    private bool isRespawning = false;
    private float timeRemaining;

    void Start()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        timeRemaining = respawnTime;

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (playerVisuals == null)
            Debug.LogWarning("Player visuals not assigned in PlayerDeath script!");
    }

    void Update()
    {
        if (isRespawning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            countdownText.text = $"Respawning in {Mathf.CeilToInt(timeRemaining)}";

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isRespawning = false;
                countdownText.gameObject.SetActive(false);
                Respawn();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead || isRespawning) return;

        if (other.CompareTag("EnemyHole") && other.transform.localScale.x > transform.localScale.x)
        {
            StartCoroutine(DieAndRespawn());
        }
    }

    IEnumerator DieAndRespawn()
    {
        isDead = true;
        isRespawning = true;
        timeRemaining = respawnTime;

        // Hide only visuals (NOT the whole GameObject)
        if (playerVisuals != null)
            playerVisuals.SetActive(false);

        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        while (isRespawning && timeRemaining > 0)
        {
            yield return null;
        }

        // Respawn is triggered in Update() when timeRemaining <= 0
    }

    void Respawn()
    {
        transform.position = initialPosition;
        transform.localScale = initialScale;

        if (playerVisuals != null)
            playerVisuals.SetActive(true);

        isDead = false;
        isRespawning = false;

        Debug.Log("Player respawned.");
    }
}
