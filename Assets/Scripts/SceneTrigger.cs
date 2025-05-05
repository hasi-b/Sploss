using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    public Animator transitionAnimator;
    private int playerEntryCount = 0;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Level 4")
            {
                // Play transition animation
                if (transitionAnimator != null)
                    transitionAnimator.SetTrigger("PlayTransition");
                // Fade out player
                StartCoroutine(FadeOutPlayer(other.GetComponent<SpriteRenderer>(), 15f));
                // Load next scene after animation
                StartCoroutine(LoadSceneAfterDelay(20f));
            }
            else if (SceneManager.GetActiveScene().name == "Level 3")
            {
                playerEntryCount++;
                if (playerEntryCount == 2)
                {
                    triggered = true;

                    // Disable movement script
                    MonoBehaviour
                        movementScript =
                            other.GetComponent<
                                MonoBehaviour>(); // Replace with actual movement script if named differently
                    if (movementScript != null)
                        movementScript.enabled = false;

                    // Freeze Rigidbody2D movement immediately
                    Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = Vector2.zero;
                        rb.isKinematic = true; // Optional: stops all physics response
                    }

                    // Play transition animation
                    if (transitionAnimator != null)
                        transitionAnimator.SetTrigger("PlayTransition");

                    // Fade out player
                    StartCoroutine(FadeOutPlayer(other.GetComponent<SpriteRenderer>(), 12f));

                    // Load next scene after animation
                    StartCoroutine(LoadSceneAfterDelay(6f));
                }
            }
        }
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator FadeOutPlayer(SpriteRenderer sr, float duration)
    {
        if (sr == null) yield break;

        Color startColor = sr.color;
        float time = 0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / duration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}
