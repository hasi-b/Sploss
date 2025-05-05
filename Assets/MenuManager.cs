using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = Unity.Mathematics.Random;

public class MenuManager : MonoBehaviour
{
    public GameObject CreditsPanel;
    public GameObject QuestionPanel;
    public TextMeshProUGUI QuestionsText;
    public TextMeshProUGUI[] AnswersText;

    public int Choice;

    [SerializeField]
    private string[] Questions;
    private string[,] Answers = new string[5, 4];
    [SerializeField]
    List<QuestionData> questionList;
    
    [SerializeField]
    ChoiceData choiceData;


    


    private void Start()
    {
        //Answers[0,0] = "I feel lost, like I can’t find my way.";
        //Answers[0,1] = "I feel angry, like I want to break through everything in front of me.";
        //Answers[0,2] = "I feel tired, as though the weight of the world is on my shoulders.";
        //Answers[0,3] = "I feel ready to take a step forward, no matter how small.";
        //Answers[1,0] = "It’s okay to be scared. Nothing is stronger than accepting your own fear.";
        //Answers[1,1] = "You’re stronger than you know.";
        //Answers[1,2] = "This isn’t your fault.";
        //Answers[1,3] = "I wish I could hold you.";
        //Answers[2,0] = "In my heart—it aches but remembers.";
        //Answers[2,1] = "In my mind—thoughts I can’t escape.";
        //Answers[2,2] = "In my body—everything feels heavy.";
        //Answers[2,3] = "Everywhere—it surrounds me.";
        //Answers[3,0] = "A storm cloud hovering above me.";
        //Answers[3,1] = "A maze I can’t seem to solve.";
        //Answers[3,2] = "A mountain that feels impossible to climb.";
        //Answers[3,3] = "A river that I must eventually cross.";
        //Answers[4,0] = "To find peace, even for a moment.";
        //Answers[4,1] = "To feel connected to those I’ve lost.";
        //Answers[4,2] = "To gather the strength to keep going.";
        //Answers[4,3] = "To make sense of all this.";
    }


    




    public void StartGame()
    {
        int random = UnityEngine.Random.Range(0, questionList.Count);
        QuestionPanel.SetActive(true);
        QuestionsText.text = questionList[random].Question; //Questions[random];
        for (int i = 0; i < questionList[random].answerOptions.Count; i++)
        {
            AnswersText[i].text = questionList[random].answerOptions[i].answer;
        }
    }

    public void StartLevel()
    {
        Choice = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        choiceData.choiceValue = Choice;
        SceneManager.LoadScene("Level 1");
    }

    public void ShowCredits()
    {
        if(CreditsPanel.activeInHierarchy)
            CreditsPanel.SetActive(false);
        else
            CreditsPanel.SetActive(true);
    }
    public void EndGame()
    {
        Application.Quit();
    }
}