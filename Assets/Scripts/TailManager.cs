using System.Collections.Generic;
using UnityEngine;

public class TailManager : MonoBehaviour
{
    public float tailSpacing = 0.5f; // Spacing between each object in the tail
    public float tailFollowSpeed = 5f; // Speed at which tail objects follow the player
    public Transform tailParent; // Parent object for organizing tail elements
    private List<Vector3> tailPositions = new List<Vector3>(); // List to store previous positions
    private List<Transform> tailObjects = new List<Transform>(); // List to store tail objects

    public int TailObjectCount => tailObjects.Count; // Getting method for Camera Zoom out script
    
    private void Start()
    {
        // Add the player's initial position as the starting point for the tail
        tailPositions.Add(transform.position);
    }

    private void Update()
    {
        // Record the player's current position for tail objects to follow
        if (Vector3.Distance(transform.position, tailPositions[0]) > tailSpacing)
        {
            tailPositions.Insert(0, transform.position);
        }

        // Keep only positions relevant to the number of tail objects
        while (tailPositions.Count > tailObjects.Count + 1)
        {
            tailPositions.RemoveAt(tailPositions.Count - 1);
        }

        // Move each tail object towards its target position
        for (int i = 0; i < tailObjects.Count; i++)
        {
            if ( i+1< tailPositions.Count) {
                Vector3 targetPosition = tailPositions[i + 1];
                tailObjects[i].position = Vector3.Lerp(tailObjects[i].position, targetPosition, tailFollowSpeed * Time.deltaTime);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object has a specific tag to be collected
        if (collision.CompareTag("Collectible"))
        {
            AddToTail(collision.transform);
        }
    }

    private void AddToTail(Transform newTailObject)
    {
        // Add the new object to the tail
        newTailObject.tag = "Untagged"; // Prevent duplicate collection
        newTailObject.gameObject.layer = LayerMask.NameToLayer("Default"); // Optionally set layer to avoid collision issues
        newTailObject.parent = tailParent; // Set the tail parent
        tailObjects.Add(newTailObject);

        // Reset object physics and position
        Rigidbody2D rb = newTailObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        Collider2D col = newTailObject.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Start the new object at the last tail position
        Vector3 lastPosition = tailObjects.Count > 1 ? tailObjects[tailObjects.Count - 2].position : transform.position;
        newTailObject.position = lastPosition;
    }
}
