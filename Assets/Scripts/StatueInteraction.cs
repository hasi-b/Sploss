using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class StatueInteraction : MonoBehaviour
{
    public Image interactionImage; // The unique image for this statue's interaction
    public TextMeshProUGUI interactionText; // Text nested in the image
    public Button interactionButton1, interactionButton2; // Buttons nested in the image
    public float panelFadeDuration = 1.5f; // Duration for the image to fade in/out
    public float statueFadeDuration = 2f; // Duration for the statue to fade out
    public KeyCode interactionKey = KeyCode.Space; // Key to initiate interaction
    public Color defaultTextColor = Color.white; // Default color of button text
    public Color hoverTextColor = Color.yellow; // Hover color for button text

    private bool isInteracting = false; // Is the player currently interacting with this statue
    private GameObject player; // Reference to the player GameObject
    private bool playerNearby = false; // Is the player in interaction range

    private void Start()
    {
        // Ensure this statue has a BoxCollider2D for detecting the player
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;

        // Add hover effects to buttons
        AddHoverEffects(interactionButton1);
        AddHoverEffects(interactionButton2);
    }

    private void Update()
    {
        // Check for interaction if the player is nearby and not already interacting
        if (playerNearby && Input.GetKeyDown(interactionKey) && !isInteracting)
        {
            StartCoroutine(HandleInteraction());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Called enter");
        // Detect when the player enters the statue's range
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Called exit");
        // Detect when the player exits the statue's range
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            player = null;
        }
    }

    private IEnumerator HandleInteraction()
    {
        isInteracting = true;

        // Disable player movement
        if (player.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            playerMovement.enabled = false;
        }

        // Activate and fade in the interaction image
        interactionImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeImage(interactionImage, 0f, 1f, panelFadeDuration));

        // Wait for one of the buttons to be clicked
        bool buttonClicked = false;
        interactionButton1.onClick.AddListener(() => buttonClicked = true);
        interactionButton2.onClick.AddListener(() => buttonClicked = true);
        yield return new WaitUntil(() => buttonClicked);

        // Fade out the interaction image
        yield return StartCoroutine(FadeImage(interactionImage, 1f, 0f, panelFadeDuration));
        interactionImage.gameObject.SetActive(false);

        // Remove button listeners to avoid memory leaks
        interactionButton1.onClick.RemoveAllListeners();
        interactionButton2.onClick.RemoveAllListeners();

        // Fade out the statue
        SpriteRenderer statueRenderer = GetComponent<SpriteRenderer>();
        if (statueRenderer != null)
        {
            yield return StartCoroutine(FadeSpriteRenderer(statueRenderer, 1f, 0f, statueFadeDuration));
        }
        // Re-enable player movement
        if (player.TryGetComponent<PlayerMovement>(out playerMovement))
        {
            playerMovement.enabled = true;
        }

        // Destroy the statue
        Destroy(gameObject);
        isInteracting = false;
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        Color color = image.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        image.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    private IEnumerator FadeSpriteRenderer(SpriteRenderer spriteRenderer, float startAlpha, float endAlpha, float duration)
    {
        Color color = spriteRenderer.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        spriteRenderer.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    private void AddHoverEffects(Button button)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        // Add event listeners for pointer enter and exit
        button.onClick.AddListener(() => OnButtonClick(buttonText));

        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        pointerEnter.callback.AddListener((data) => OnHoverEnter(buttonText));

        EventTrigger.Entry pointerExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        pointerExit.callback.AddListener((data) => OnHoverExit(buttonText));

        trigger.triggers.Add(pointerEnter);
        trigger.triggers.Add(pointerExit);
    }

    private void OnHoverEnter(TextMeshProUGUI buttonText)
    {
        if (buttonText != null)
        {
            buttonText.color = hoverTextColor;
        }
    }

    private void OnHoverExit(TextMeshProUGUI buttonText)
    {
        if (buttonText != null)
        {
            buttonText.color = defaultTextColor;
        }
    }

    private void OnButtonClick(TextMeshProUGUI buttonText)
    {
        // Optional: Add logic if something should happen when a button is clicked
    }
}
