using UnityEngine;

public class SpriteAnimNoLoop : MonoBehaviour
{
    public Sprite[] sprites; // Array to hold all the sprites
    public float framesPerSecond = 30f; // Number of frames per second

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float frameTimer;
    private bool isPlaying = true; // Flag to check if the animation should continue

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("No sprites assigned to the SpriteAnimator!");
        }
    }

    void Update()
    {
        if (sprites == null || sprites.Length == 0 || !isPlaying)
            return;

        // Update the timer
        frameTimer += Time.deltaTime;

        // Check if it's time to switch to the next frame
        if (frameTimer >= 1f / framesPerSecond)
        {
            frameTimer -= 1f / framesPerSecond; // Reset the timer
            currentFrame++;

            if (currentFrame >= sprites.Length)
            {
                // Once we reach the end, stop and make the sprite empty
                spriteRenderer.sprite = null;
                isPlaying = false;
                return;
            }

            spriteRenderer.sprite = sprites[currentFrame]; // Update the sprite
        }
    }
}
