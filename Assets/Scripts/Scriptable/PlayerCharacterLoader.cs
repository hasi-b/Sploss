using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterLoader : MonoBehaviour
{
    public static PlayerCharacterLoader Instance { get; private set; }
    public SpriteRenderer Character { get => character; set => character = value; }
    public ChoiceData ChoiceData { get => choiceData; set => choiceData = value; }
    public int ChoiceIndex { get => choiceIndex; set => choiceIndex = value; }
    [SerializeField]
    bool acceptanceState;
    [SerializeField]
    SpriteRenderer character;
    [SerializeField]
    ChoiceData choiceData;
    int choiceIndex;
    
    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject); // This makes the object persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    private void Start()
    {
        ChoiceIndex = ChoiceData.choiceValue;
        LoadCharacter();
    }

    private void LoadCharacter()
    {
        if (!acceptanceState)
        {
            Character.sprite = ChoiceData.spriteAnswer[ChoiceIndex - 1].initialSprite;
        }
        else
        {
            Character.sprite = ChoiceData.spriteAnswer[ChoiceIndex - 1].endSprite;
        }
       
    }
}
