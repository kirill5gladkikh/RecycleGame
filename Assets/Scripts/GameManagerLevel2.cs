using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TrashItemLevel2
{
    public Sprite sprite;
    public int correctBin; // 0=Plastic, 1=Paper, 2=Glass, 3=Trash

    public int specialWrongBin = -1;
    public string specialHint;

    public bool canBeCleaned;
    public Sprite cleanedSprite;
    public int cleanedCorrectBin;
}

public class GameManagerLevel2 : MonoBehaviour
{
    public SpriteRenderer itemRenderer;
    public DraggableItemLevel2 draggableItem;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI gameOverText;

    public TrashItemLevel2[] items;

    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public GameObject hintPanel;
    public TextMeshProUGUI hintText;

    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip okButtonSound;

    private List<TrashItemLevel2> remainingItems = new List<TrashItemLevel2>();
    private TrashItemLevel2 currentItem;

    private int coinsEarnedThisRound = 0;
    private bool isPaused = false;
    private bool hintIsOpen = false;

    void Start()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }

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

            if (draggableItem != null)
            {
                draggableItem.enabled = false;
            }

            return;
        }

        int randomIndex = Random.Range(0, remainingItems.Count);

        currentItem = remainingItems[randomIndex];
        remainingItems.RemoveAt(randomIndex);

        itemRenderer.gameObject.SetActive(true);
        itemRenderer.sprite = currentItem.sprite;

        draggableItem.ResetPosition();

        if (hintIsOpen == false && draggableItem != null)
        {
            draggableItem.enabled = true;
        }
    }

    public void Choose(int binType)
    {
        if (hintIsOpen == true)
        {
            return;
        }

        if (isPaused == true)
        {
            return;
        }

        if (binType == currentItem.correctBin)
        {
            CoinManager.coins++;
            coinsEarnedThisRound++;

            feedbackText.text = "+1 coin";

            HideHintPanel();

            if (audioSource != null && correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
            }

            coinsText.text = "Coins: " + CoinManager.coins;

            ShowRandomItem();
        }
        else
        {
            if (binType == currentItem.specialWrongBin && currentItem.specialHint != "")
            {
                feedbackText.text = "";

                if (hintPanel != null && hintText != null)
                {
                    ShowHintPanel(currentItem.specialHint);
                }
                else
                {
                    feedbackText.text = currentItem.specialHint;
                }
            }
            else
            {
                feedbackText.text = "Wrong";

                HideHintPanel();
            }

            if (audioSource != null && wrongSound != null)
            {
                audioSource.PlayOneShot(wrongSound);
            }

            draggableItem.ResetPosition();
        }
    }

    public bool TryCleanCurrentItem()
    {
        if (hintIsOpen == true)
        {
            return false;
        }

        if (isPaused == true)
        {
            return false;
        }

        if (currentItem == null)
        {
            return false;
        }

        if (currentItem.canBeCleaned == false)
        {
            feedbackText.text = "This item cannot be cleaned.";
            draggableItem.ResetPosition();
            return false;
        }

        Sprite cleanedSprite = currentItem.cleanedSprite;
        int cleanedCorrectBin = currentItem.cleanedCorrectBin;
        int specialWrongBin = currentItem.specialWrongBin;
        string specialHint = currentItem.specialHint;

        if (cleanedSprite != null)
        {
            itemRenderer.sprite = cleanedSprite;
        }

        currentItem = new TrashItemLevel2
        {
            sprite = cleanedSprite,
            correctBin = cleanedCorrectBin,

            specialWrongBin = specialWrongBin,
            specialHint = specialHint,

            canBeCleaned = false,
            cleanedSprite = cleanedSprite,
            cleanedCorrectBin = cleanedCorrectBin
        };

        feedbackText.text = "Now it is clean!";

        HideHintPanel();

        draggableItem.ResetPosition();

        return true;
    }

    void ShowHintPanel(string message)
    {
        hintIsOpen = true;

        if (hintText != null)
        {
            hintText.text = message;
        }

        if (hintPanel != null)
        {
            hintPanel.SetActive(true);
        }

        if (draggableItem != null)
        {
            draggableItem.enabled = false;
        }
    }

    void HideHintPanel()
    {
        hintIsOpen = false;

        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }

        if (draggableItem != null && gameOverPanel.activeSelf == false && isPaused == false)
        {
            draggableItem.enabled = true;
        }
    }

    public void CloseHint()
    {
        if (audioSource != null && okButtonSound != null)
        {
            audioSource.PlayOneShot(okButtonSound);
        }

        HideHintPanel();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        isPaused = false;
        hintIsOpen = false;

        coinsEarnedThisRound = 0;
        remainingItems = new List<TrashItemLevel2>(items);

        coinsText.text = "Coins: " + CoinManager.coins;
        feedbackText.text = "";

        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);

        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }

        if (draggableItem != null)
        {
            draggableItem.enabled = true;
        }

        ShowRandomItem();
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        if (draggableItem != null)
        {
            draggableItem.enabled = false;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        if (hintIsOpen == false && draggableItem != null)
        {
            draggableItem.enabled = true;
        }
    }
}