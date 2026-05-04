using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TrashItem
{
    public Sprite sprite;
    public int correctBin; // 0=Plastic, 1=Paper, 2=Glass, 3=Trash
}

public class GameManager : MonoBehaviour
{
    public SpriteRenderer itemRenderer;
    public DraggableItem draggableItem;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI gameOverText;

    public TrashItem[] items;

    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private List<TrashItem> remainingItems = new List<TrashItem>();
    private TrashItem currentItem;

    private int coinsEarnedThisRound = 0;
    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        RestartGame();
    }

    void ShowRandomItem()
    {
        if (remainingItems.Count == 0)
        {
            itemRenderer.gameObject.SetActive(false);

            feedbackText.text = "";
            gameOverText.text = "\nYou earned: " + coinsEarnedThisRound + " coins";

            gameOverPanel.SetActive(true);
            return;
        }

        int randomIndex = Random.Range(0, remainingItems.Count);

        currentItem = remainingItems[randomIndex];
        remainingItems.RemoveAt(randomIndex);

        itemRenderer.gameObject.SetActive(true);
        itemRenderer.sprite = currentItem.sprite;

        draggableItem.ResetPosition();
    }

    public void Choose(int binType)
    {
        if (binType == currentItem.correctBin)
        {
            CoinManager.coins++;
            coinsEarnedThisRound++;

            feedbackText.text = "+1 coin";

            if (audioSource != null && correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
        }
        else
        {
            feedbackText.text = "Wrong";

            if (audioSource != null && wrongSound != null)
            {
                audioSource.PlayOneShot(wrongSound);
            }
        }

        coinsText.text = "Coins: " + CoinManager.coins;

        ShowRandomItem();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        coinsEarnedThisRound = 0;
        remainingItems = new List<TrashItem>(items);

        coinsText.text = "Coins: " + CoinManager.coins;
        feedbackText.text = "";

        gameOverPanel.SetActive(false);

        ShowRandomItem();
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToLevel2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}