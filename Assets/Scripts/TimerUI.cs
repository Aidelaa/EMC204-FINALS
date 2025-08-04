using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float startTime = 60f;
    private float timeRemaining;
    private bool isCountingDown = false;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ResetTimer();
        StartCountdown();
    }

    void Update()
    {
        if (isCountingDown)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = FormatTime(timeRemaining);

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isCountingDown = false;
                timerText.text = "Time's Up!";
                Time.timeScale = 0f;
            }
        }
    }

    public void StartCountdown()
    {
        isCountingDown = true;
    }

    private void ResetTimer()
    {
        timeRemaining = startTime;
        isCountingDown = false;
        timerText.text = FormatTime(timeRemaining);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        ResetTimer();
        StartCountdown();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
