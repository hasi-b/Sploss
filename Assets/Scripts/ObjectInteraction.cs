using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public bool IsInlist = false;
    public Sprite Sprite;

    OutlineController outlineController;

    private void Start()
    {
        outlineController = GetComponent<OutlineController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (/*!IsInlist && */collision.gameObject.CompareTag("Player"))
        {
           outlineController.SetOutline();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (/*!IsInlist &&*/ collision.gameObject.CompareTag("Player"))
        {
            outlineController.SetOutline();
        }
    }
}
