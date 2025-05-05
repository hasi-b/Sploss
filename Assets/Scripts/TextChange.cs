using System.Collections;
using UnityEngine;
using TMPro;

public class TextChange : MonoBehaviour
{
    public TMP_Text textMeshProObject; // Assign the TextMeshPro object in the Inspector
    public string[] messages; // Array of messages to display (6 messages required)
    private int currentIndex = 0; // To keep track of the current message index
    public float timer = 3f;
    public bool flag;
    void Start()
    {
        if(!flag)
            StartCoroutine(ChangeTextRoutine());
    }

    public IEnumerator ChangeTextRoutine()
    {
        while (currentIndex < messages.Length) // Repeat 6 times (30 seconds total)
        {
            textMeshProObject.text = messages[currentIndex]; // Set the text
            currentIndex++; // Move to the next message
            yield return new WaitForSeconds(timer); // Wait for 5 seconds
        }
    }
}
