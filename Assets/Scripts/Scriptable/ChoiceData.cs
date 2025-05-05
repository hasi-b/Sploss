using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChoiceData", menuName = "Custom/ChoiceData")]
public class ChoiceData : ScriptableObject
{
    public int choiceValue;
    public List<SpriteData> spriteAnswer;
}
