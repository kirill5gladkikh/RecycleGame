using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    public GameManagerLevel2 gameManager;

    public AudioSource audioSource;
    public AudioClip waterSound;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayWaterSound()
    {
        if (audioSource != null && waterSound != null)
        {
            audioSource.PlayOneShot(waterSound);
        }
    }
}