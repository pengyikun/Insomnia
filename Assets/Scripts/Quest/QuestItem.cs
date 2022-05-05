using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem 
{
    public int id;
    public Quest quest;
    public bool completed;

    public QuestItem(Quest q)
    {
        id = q.id;
        quest = q;
        completed = false;
    }
}
