using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    public GameObject inventoryUI;
    public GameObject playerInfoUI;
    public GameObject playerStatsUI;
    public GameObject playerStatusEffectsUI;
    public GameObject questPanelUI;
    public GameObject InGameMenuUI;
    public GameObject instructionsMenuUI;
    public GameObject overMenu;

    public Transform playerStatsContent;
    public Transform playerInfoContent;
    public Transform playerStatusEffectsContent;
    public GameObject playerStatusEffectPrefab;
    public GameObject waveIndicatorContent;
    public Transform NotificationsPanelContent;
    
    public GameObject NotificationPrefab;

    public GameObject crossHair;
    
    PlayerStats playerStats;
    private CharacterSoundController characterSoundController;

    private Text level;
    private Text weight;
    private Text experience;
    private Text maxHealth;
    private Text maxStamina;
    private Text gold;
    private Text health;
    private Text stamina;

    public Text weaponMode;
    public Text ammoCount;

    private AudioSource audioSrc;
    public AudioClip uiSound;


    private void Awake()
    {
        Instance = this;
        playerStats = FindObjectOfType<PlayerController>().GetComponent<PlayerStats>();
        characterSoundController = FindObjectOfType<PlayerController>().GetComponent<CharacterSoundController>();
        level = playerInfoContent.transform.Find("Level").GetComponent<Text>();
        weight = playerInfoContent.transform.Find("Weight").GetComponent<Text>();
        experience = playerInfoContent.transform.Find("Exp").GetComponent<Text>();
        maxHealth = playerInfoContent.transform.Find("MaxHealth").GetComponent<Text>();
        maxStamina = playerInfoContent.transform.Find("MaxStamina").GetComponent<Text>();
        gold = playerInfoContent.transform.Find("Gold").GetComponent<Text>();
        health = playerStatsContent.transform.Find("Health").GetComponent<Text>();
        stamina = playerStatsContent.transform.Find("Stamina").GetComponent<Text>();
        audioSrc = GetComponent<AudioSource>();
    }

    public void UpdatePlayerInfoUI()
    {
        level.text = playerStats.level.ToString();
        weight.text = playerStats.weight.ToString() + "/" + playerStats.maxWeight.ToString();
        experience.text = playerStats.exp.ToString() + "/" + playerStats.levelUpExp.ToString();
        maxHealth.text = playerStats.maxHealth.ToString();
        maxStamina.text = playerStats.maxStamina.ToString();
        gold.text = playerStats.gold.ToString();
    }

    public void UpdatePlayerStatsUI()
    {
        health.text = playerStats.health.ToString("0.00");
        stamina.text = playerStats.stamina.ToString("0.00");
        UpdatePlayerStatusEffectsUI();
        UpdateWeaponStatsUI();
    }

    public void UpdateWeaponStatsUI()
    {
        weaponMode.text = playerStats.GetWeaponMode() == 0 ? "Rifle(-1)" : "Cannon(-5)";
        ammoCount.text = playerStats.GetAmmo().ToString();
    }

    public void UpdatePlayerStatusEffectsUI()
    {
        foreach (Transform item in playerStatusEffectsContent)
        {
            Destroy(item.gameObject);
        }
        
        foreach (StatusEffect statusEffect in playerStats.GetStatusEffectsList())
        {
            GameObject obj = Instantiate(playerStatusEffectPrefab, playerStatusEffectsContent);
            var effectName = obj.transform.Find("StatusEffectName").GetComponent<Text>();
            var effectLeftTime = obj.transform.Find("StatusEffectTime").GetComponent<Text>();

            float timeLeft = statusEffect.effectData.lifeTime - statusEffect.currentEffectTime; 
            
            effectName.text = statusEffect.effectData.effectName;
            effectLeftTime.text = timeLeft < 1000 ? timeLeft.ToString("0.00"): "-";
        }
    }

    public void TogglePlayerInfoUI()
    {
        bool active = inventoryUI.activeSelf;
        ToggleCursor(!active);
        if (!active)
        {
            playerStats.canFire = false;
            ClearUI();
            inventoryUI.SetActive(true);
            playerInfoUI.SetActive(true);
            InventoryManager.Instance.UpdateUI();
            UpdatePlayerInfoUI();
            characterSoundController.PlayOpenInventorySound();
        }
        else
        {
            playerStats.canFire = true;
            ClearUI(); 
            characterSoundController.PlayCloseInventorySound();
        }
    }

    public void ToggleQuestPanelUI()
    {
        audioSrc.PlayOneShot(uiSound);
        bool active = questPanelUI.activeSelf;
        ToggleCursor(!active);
        if (!active)
        {
            playerStats.canFire = false;
            ClearUI();
            questPanelUI.SetActive(!questPanelUI.activeSelf);
            QuestManager.Instance.UpdateUI();
            //characterSoundController.PlayNotificationSound();
        }
        else
        {
            playerStats.canFire = true;
            ClearUI(); 
           //characterSoundController.PlayNotificationSound();
        }
    }

    public void ToggleInGameMenuUI()
    {
        bool active = InGameMenuUI.activeSelf;
        ToggleCursor(!active);
        if (!active)
        {
            playerStats.canFire = false;
            ClearUI();
            InGameMenuUI.SetActive(!InGameMenuUI.activeSelf);
        }
        else
        {
            playerStats.canFire = true;
            ClearUI(); 
        }
    }

    public void ToggleInstrctionsMenuUI()
    {
        bool active = instructionsMenuUI.activeSelf;
        ToggleCursor(!active);
        if (!active)
        {
            playerStats.canFire = false;
            ClearUI();
            instructionsMenuUI.SetActive(!instructionsMenuUI.activeSelf);
        }
        else
        {
            playerStats.canFire = true;
            ClearUI(); 
        }
    }

    public void ToggleOverMenu()
    {
        ClearUI();
        overMenu.SetActive(true);
        ToggleCursor(true);
        Text label = overMenu.transform.Find("Label").GetComponent<Text>();
        if (playerStats.gameStates == "success")
        {
            label.text = "Mission Success";
            label.color = Color.green;
        }
        else
        {
            label.text = "Mission Failed";
            label.color = Color.red; 
        }
    }

    public void ToggleWaveIndicatorUI(bool visible)
    {
        waveIndicatorContent.SetActive(visible);
    }

    public void UpdateWaveIndicatorText(string text)
    {
        Text t = waveIndicatorContent.transform.Find("WaveCount").GetComponent<Text>();
        t.text = text;
    }

    public void ClearUI()
    {
        inventoryUI.SetActive(false);
        playerInfoUI.SetActive(false);
        playerStatusEffectsUI.SetActive(true); 
        questPanelUI.SetActive(false);
        InGameMenuUI.SetActive(false);
        instructionsMenuUI.SetActive(false);
    }
    

    public bool IsInteractiveUIOpened()
    {
        return inventoryUI.activeSelf || playerInfoUI.activeSelf || questPanelUI.activeSelf;
    }
    

    public void ToggleCrossHair(bool visible)
    {
        crossHair.SetActive(visible);
    }
    
    public void ToggleCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None :  CursorLockMode.Locked;
        //Debug.Log(Cursor.visible);
    }

    public void PushNotification(string content)
    {
        GameObject obj = Instantiate(NotificationPrefab, NotificationsPanelContent); 
        obj.GetComponent<NotificationItemController>().SetContent(content);
    }

    public CharacterSoundController GetCharacterSoundController()
    {
        return characterSoundController;
    }
}
