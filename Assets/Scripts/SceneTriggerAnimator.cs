using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTriggerAnimator : MonoBehaviour
{
    public string sceneToLoad = "Level 5"; // Name of the scene to load
    public SpriteRenderer animationRenderer; // Reference to the SpriteRenderer for the animation
    public Sprite[] animationFrames; // Array of sprites for the animation
    public float animationFrameRate = 45f; // Frame rate for the animation

    private bool isAnimating = false; // Whether the animation is currently playing

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is the player
        {
            Debug.Log("Player entered trigger. Playing animation...");
            StartCoroutine(PlayAnimationAndLoadScene());
        }
    }

    private System.Collections.IEnumerator PlayAnimationAndLoadScene()
    {
        isAnimating = true;
        float frameTime = 1f / animationFrameRate;

        // Play the animation frame by frame
        for (int i = 0; i < animationFrames.Length; i++)
        {
            animationRenderer.sprite = animationFrames[i]; // Set the current sprite
            yield return new WaitForSeconds(frameTime); // Wait for the next frame
        }

        Debug.Log("Animation finished. Loading scene: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad); // Load the next scene after the animation
    }
}
