using UnityEngine;
using UnityEngine.SceneManagement;

public class PathFollower : MonoBehaviour
{
    public Transform[] pathPoints; // Array to store path points
    public float initialMoveSpeed = 2f; // Initial movement speed of the character
    public float minMoveSpeed = 0.5f; // Minimum movement speed
    public float shrinkSpeed = 0.05f; // Rate at which the character shrinks
    public Vector3 minimumScale = new Vector3(0.1f, 0.1f, 0.1f); // Minimum size the character can shrink to

    private int currentPointIndex = 0; // Current point the character is heading to
    private bool isWalking = false; // Whether the character is moving
    private bool reachedFinalPoint = false; // Whether the final point is reached

    private float currentMoveSpeed; // Current dynamic movement speed
    private SpriteRenderer spriteRenderer;

    public GameObject pauseMenuPanel;
    private bool isPaused = false;
    [SerializeField]
    Animator animator;
    void Start()
    {
        currentMoveSpeed = initialMoveSpeed; // Set the initial speed
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();


        // Replace a specific clip (e.g., "Idle" is the name of the original motion)
        Debug.Log("number: " + PlayerCharacterLoader.Instance.ChoiceIndex);
        animator.runtimeAnimatorController = PlayerCharacterLoader.Instance.ChoiceData.spriteAnswer[PlayerCharacterLoader.Instance.ChoiceIndex-1 ].controller;
       
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
        // Start walking when W is pressed and there are remaining points
        if (Input.GetKey(KeyCode.W) && currentPointIndex < pathPoints.Length && !reachedFinalPoint)
        {
            isWalking = true;
        }

        if (isWalking)
        {
            MoveTowardsPoint();
        }
    }

    private void MoveTowardsPoint()
    {
        // Get the target point
        Transform targetPoint = pathPoints[currentPointIndex];

        // Calculate the distance to the target point
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);

        // Adjust the move speed based on the distance, ensuring it doesn't go below the minimum speed
        currentMoveSpeed = Mathf.Max(minMoveSpeed, initialMoveSpeed * (distanceToTarget / 10f));

        // Move towards the target point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, currentMoveSpeed * Time.deltaTime);

        // Shrink the character gradually but not below the minimum size
        transform.localScale = Vector3.Max(
            Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime),
            minimumScale
        );  
        
        // Handle transparency between point 3 and 4
        if (spriteRenderer != null && currentPointIndex == 3 && pathPoints.Length > 4)
        {
            Transform point3 = pathPoints[3];
            Transform point4 = pathPoints[4];
            float totalDist = Vector3.Distance(point3.position, point4.position);
            float distFrom3 = Vector3.Distance(transform.position, point4.position); // Distance remaining to point 4

            float t = Mathf.InverseLerp(totalDist, 0f, distFrom3); // 0 at point3, 1 at point4
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = color;
        }

        // Check if the character has reached the target point
        if (distanceToTarget < 0.1f)
        {
            currentPointIndex++; // Move to the next point

            // If the final point is reached
            if (currentPointIndex >= pathPoints.Length)
            {
                isWalking = false;
                reachedFinalPoint = true;

                // Attach character to the final path point to follow its movement
                Transform finalPoint = pathPoints[pathPoints.Length - 1];
                transform.parent = finalPoint;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Optional: Draw gizmos to visualize the path
        Gizmos.color = Color.red;
        for (int i = 0; i < pathPoints.Length - 1; i++)
        {
            Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
        }
    }
    
    private void PauseGame()
    {
        Time.timeScale = 0f; // Freeze game time
        pauseMenuPanel.SetActive(true); // Show pause menu
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume game time
        pauseMenuPanel.SetActive(false); // Hide pause menu
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
