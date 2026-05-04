using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip playSound;

    public void PlayGame()
    {
        StartCoroutine(PlaySoundAndLoadGame());
    }

    private IEnumerator PlaySoundAndLoadGame()
    {
        if (audioSource != null && playSound != null)
        {
            audioSource.PlayOneShot(playSound);
            yield return new WaitForSeconds(playSound.length);
        }

        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}