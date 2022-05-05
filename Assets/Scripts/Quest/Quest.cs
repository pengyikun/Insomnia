using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewQuest", menuName ="Quest/CreateNewQuest")]

public class Quest: ScriptableObject
{
    public int id;
    public string questName;
    [TextArea]
    public string questDescription;
    public Sprite image;
    public int gold;
    public float exp;
}
