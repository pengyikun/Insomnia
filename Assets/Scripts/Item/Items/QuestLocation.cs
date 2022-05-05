using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocation : MonoBehaviour,IInteractable
{
    public bool IsActive { get; set; }
    [SerializeField] private Quest quest;

    private void Start()
    {
        IsActive = false;
        transform.Find("QuestTitleText").GetComponent<TextMesh>().text = quest.questName;
    }

    public void OnInteract(GameObject obj)
    {
        if (QuestManager.Instance.CompleteQuest(quest.id))
        {
            Destroy(this.gameObject); 
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnInteract(other.gameObject);
        } 
    }
}
