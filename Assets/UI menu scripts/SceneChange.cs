using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public GameObject credits;
    public Animator sceneloader;
    public float transitionTime = 1f;
    public AudioClip buttonSound; // Audio clip to play when loading next game
    private AudioSource audioSource;

    private void Start()
    {
        credits.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void loadnextGameScene()
    {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void loadMainMenu()
    {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    IEnumerator loadLevel(int levelIndex)
    {
        sceneloader.SetTrigger("start");

        // Play audio clip
        if (buttonSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonSound);
            yield return new WaitForSeconds(buttonSound.length); // Wait for audio clip to finish
        }

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void ShowCredits()
    {
        credits.SetActive(true);
        audioSource.PlayOneShot(buttonSound);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
        audioSource.PlayOneShot(buttonSound);
    }
    public void exitAPP()
    {
        Application.Quit();
    }
}
