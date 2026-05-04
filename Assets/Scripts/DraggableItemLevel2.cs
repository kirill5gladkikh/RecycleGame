using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DraggableItemLevel2 : MonoBehaviour
{
    public GameManagerLevel2 gameManager;
    public Transform spawnPoint;

    public AudioSource audioSource;
    public AudioClip[] grabSounds;

    private bool isDragging = false;
    private Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        ResetPosition();
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            isDragging = false;
            return;
        }

        if (Mouse.current == null)
        {
            return;
        }

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition);

            if (hit == myCollider)
            {
                isDragging = true;

                if (audioSource != null && grabSounds.Length > 0)
                {
                    int randomIndex = Random.Range(0, grabSounds.Length);
                    audioSource.PlayOneShot(grabSounds[randomIndex]);
                }
            }
        }

        if (Mouse.current.leftButton.isPressed && isDragging)
        {
            transform.position = mouseWorldPosition;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                ResetPosition();
                return;
            }

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);

            foreach (Collider2D hit in hits)
            {
                WaterDrop water = hit.GetComponent<WaterDrop>();

                if (water != null)
                {
                    bool wasCleaned = water.gameManager.TryCleanCurrentItem();

                    if (wasCleaned == true)
                    {
                        water.PlayWaterSound();
                    }

                    return;
                }
            }

            foreach (Collider2D hit in hits)
            {
                Bin bin = hit.GetComponent<Bin>();

                if (bin != null)
                {
                    gameManager.Choose(bin.binType);
                    return;
                }
            }

            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        transform.position = spawnPoint.position;
    }
}