using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "QuestionData", menuName = "Custom/QuestionData")]
public class QuestionData : ScriptableObject

{

    public string Question;
    public List<AnswerData> answerOptions;
}
   
