using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestItemController : MonoBehaviour
{
    Quest quest;




    public void SetFocusedItem()
    {
       QuestManager.Instance.SetFocusedQuest(quest); 
    }

    public void SetQuest(Quest q)
    {
        quest = q;
    }
}
