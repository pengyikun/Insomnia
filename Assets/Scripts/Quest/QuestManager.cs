using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<Quest> questDataList = new List<Quest>();

    private Dictionary<int, QuestItem> quests = new Dictionary<int, QuestItem>();
    private QuestItem focusedQuest;
    private List<int> completedQuestIds = new List<int>();

    public Transform questPanelContent;
    public Transform focusedQuestContent;
    public GameObject questItemPrefab;
    
    PlayerController playerController;
    PlayerStats playerStats;
    private InventoryManager inventoryManager;

    private UIManager uiManager;

    public GameObject signalTransmitter;
    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerStats =playerController.gameObject.GetComponent<PlayerStats>();
        uiManager = UIManager.Instance;
        inventoryManager = InventoryManager.Instance;
        AddQuest(questDataList[0].id);
        // AddQuest(questDataList[1]);
        // AddQuest(questDataList[2]);
        // AddQuest(questDataList[3]);
        // AddQuest(questDataList[4]);
        // AddQuest(questDataList[5]);
        // AddQuest(questDataList[6]);
    }

    public void AddQuest(int questId)
    {
        if (quests.ContainsKey(questId))
        {
            Debug.Log("Can not add duplicated quest");
            return;
        }
        else
        {
            Debug.Log($"Added new quest {questId}");
            QuestItem newQuestItem = new QuestItem(questDataList[questId]);
            quests.Add(newQuestItem.id, newQuestItem);
        }
        
        UpdateQuestListUI();
        UpdateFocusedQuestUI();
    }
    
    public void RemoveQuest(int questId)
    {
        if (!quests.ContainsKey(questId))
        {
            Debug.Log("Can not delete inactive quest");
        }
        else
        {
            Debug.Log($"Removed quest {questId}");
            quests.Remove(questId);
            UpdateQuestListUI();
            UpdateFocusedQuestUI();
        }
    }

    public bool CompleteQuest(int questId)
    {
        if (!quests.ContainsKey(questId))
        {
            Debug.Log("Can not complete inactive quest");
            uiManager.PushNotification($"This quest is not active yet");
            return false;
        }
        else
        {
            if (!IsQuestCompletable(questId))
            {
                uiManager.PushNotification($"Quest {quests[questId].quest.questName} complete requirnment not met");
                return false;
            } 
            Debug.Log($"Completed quest {questId}");
            uiManager.PushNotification($"Completed quest {quests[questId].quest.questName}"); 
            completedQuestIds.Add(questId);
            playerStats.AddExp(quests[questId].quest.exp);
            playerStats.AddGold(quests[questId].quest.gold);
            playerController.PlayParticle("questComplete");
            if (focusedQuest != null && focusedQuest.id == questId) focusedQuest = null;
           RemoveQuest(questId);
           NextQuest(questId).ForEach(qid=>AddQuest(qid));
           return true;

        } 
    }

    public void SetFocusedQuest(Quest quest)
    {
        focusedQuest = quests[quest.id];
        UpdateFocusedQuestUI();
    }
    
    
    public void UpdateQuestListUI()
    {
        foreach (Transform item in questPanelContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var questItem in quests)
        {
            Quest quest = questItem.Value.quest;
            string questName = quest.questName;
            GameObject obj = Instantiate(questItemPrefab, questPanelContent);
            var questNameTextUI = obj.transform.Find("ItemName").GetComponent<Text>();
            questNameTextUI.text = questName;
            obj.GetComponent<QuestItemController>().SetQuest(quest);
        }
        
    }
    
    public void UpdateFocusedQuestUI()
    {
        var questImage = focusedQuestContent.transform.Find("QuestImage").GetComponent<Image>();
        var questDescription = focusedQuestContent.transform.Find("QuestDescription").GetComponentInChildren<Text>();
        
        if (focusedQuest == null)
        {
            if (quests.Count > 0)
            {
                List<int> l = quests.Keys.ToList();
                SetFocusedQuest(quests[l[0]].quest);
            }
            else
            {
                questImage.sprite = null;
                questDescription.text = "";
                focusedQuestContent.gameObject.SetActive(false);
                return;
            }
        }
        focusedQuestContent.gameObject.SetActive(true);
        questImage.sprite = focusedQuest.quest.image; 
        questDescription.text = focusedQuest.quest.questDescription;
            
    }

   private List<int> NextQuest(int questId){
       
        List<int> qids = new List<int>();
        switch (questId)
        {
            case 0:
                WaveManager.Instance.questOpened = true;
                uiManager.ToggleWaveIndicatorUI(true);
                WaveManager.Instance.StartWave();
                qids.Add(1);
                qids.Add(2);
                qids.Add(3);
                qids.Add(4);
                uiManager.PushNotification($"You have some new quests"); 
                break;
            case 1:
                WaveManager.Instance.questOpened = false;
                uiManager.UpdateWaveIndicatorText("Wave Completed");
                uiManager.ToggleWaveIndicatorUI(false);
                Destroy(signalTransmitter);
                break;
            case 4:
                qids.Add(5);
                uiManager.PushNotification($"You have a new quest"); 
                break;
            case 5:
                // Consume and remove gas can
                inventoryManager.RemoveItem(inventoryManager.items[5].Item, false);
                // Check if final quest is ready to activate
                if (completedQuestIds.Contains(1) && completedQuestIds.Contains(2) && completedQuestIds.Contains(3))
                {
                    qids.Add(6);
                    uiManager.PushNotification($"You have a new quest"); 
                }
                break;
            case 6:
                Debug.Log("All quest completed!");
                uiManager.PushNotification($"You have completed all quests"); 
                playerController.OnSuccess();
                break;
        }

        if (qids.Count > 0)
        {
            playerController.GetComponent<CharacterSoundController>().PlayGenericAnnouncerSound();
        }
        return qids;
   }


   public bool IsQuestCompletable(int qid)
   {
       bool completable = true;
       switch (qid)
       {
           case 5:
               completable = inventoryManager.HasItem(5); 
               break;
       }

       return completable;
   }

   public void OnAIKilledRewards(int aiLevel)
   {
       playerStats.AddExp(aiLevel * 5);
   }

    public void UpdateUI()
    {
        UpdateQuestListUI();
        UpdateFocusedQuestUI();
    }
}
