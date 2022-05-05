using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public List<GameObject> inputZombieTypes = new List<GameObject>();
    private Dictionary<string, GameObject> zombieTypes = new Dictionary<string, GameObject>();
    public List<AISpawner> aiSpawners = new List<AISpawner>();
    
    public int waveIndex = 0;
    private  Dictionary<string, int> waveZombieData = new Dictionary<string, int>();
    private int waveAICount = 0;

    private bool waveSpawning = false;
    private bool waveStarted = false;
    public bool questOpened = false;

    private PlayerController playerController;
    public SignalTransmitter signalTransmitter;
    
    
    private void Awake()
    {
        Instance = this;
        inputZombieTypes.ForEach(gameObject =>
        {
            zombieTypes.Add(gameObject.name, gameObject);
        });
        //StartWave();
    }

    private void Start()
    {
       aiSpawners.ForEach(spawner =>
       {
           spawner.zombieTypes = zombieTypes;
       }); 
       //UIManager.Instance.ToggleWaveIndicatorUI(true);
       playerController = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (waveStarted)
        {
            if (waveSpawning)
            {
                UIManager.Instance.UpdateWaveIndicatorText($"Wave {waveIndex} | Spawning");
                waveSpawning = IsWaveSpawning();
            }
            else
            {
                waveStarted = !IsAllAIKilled();
                if (!waveStarted)
                {
                    //StartWave();
                    if (waveIndex < 4)
                    {
                        UIManager.Instance.UpdateWaveIndicatorText($"Press N to activate next wave");
                        signalTransmitter.health += signalTransmitter.maxHealth % 100;
                        signalTransmitter.UpdateHealthIndicator();
                    }
                    else
                    {
                        QuestManager.Instance.CompleteQuest(1);
                    }
                }
            }
        }
  
    }

    public void StartWave()
    {
        if (waveStarted || !questOpened)
        {
            return;
        }

        PlayWaveToggleSound();
        NextWave();
        UIManager.Instance.UpdateWaveIndicatorText($"Wave {waveIndex}");
        Dictionary<string, int> spawnerZombieData = new Dictionary<string, int>();
        foreach (var data in waveZombieData)
        {
            spawnerZombieData[data.Key] = data.Value / aiSpawners.Count;
        }
        
        aiSpawners.ForEach(spawner =>
        {
            spawner.UpdateWaveData(spawnerZombieData);
            spawner.StartSpanwing();
        });
        waveStarted = true;
        waveSpawning = true;

    }

    private void NextWave()
    {
        waveIndex += 1;
        waveZombieData = new Dictionary<string, int>();
        switch (waveIndex)
        {
            case 1:
                waveZombieData.Add("ZombieDefaultWaypoint", 10);
                waveZombieData.Add("ZombieBiosuitWaypoint", 3);
                waveZombieData.Add("ZombieArmyWaypoint", 3);
                break;
            case 2:
                waveZombieData.Add("ZombieBiosuitWaypoint", 10);
                waveZombieData.Add("ZombieDefaultWaypoint", 30);
                waveZombieData.Add("ZombieArmyWaypoint", 10);
                break;
            case 3:
                waveZombieData.Add("ZombieArmyWaypoint", 30);
                waveZombieData.Add("ZombieBiosuitWaypoint", 25);
                waveZombieData.Add("ZombieDefaultWaypoint", 30);
                break;
            case 4:
                waveZombieData.Add("ZombieBiosuitWaypoint", 100);
                waveZombieData.Add("ZombieArmyWaypoint", 100); 
                waveZombieData.Add("ZombieDefaultWaypoint", 100);
                break;
        }

        UpdateWaveAICount();
    }

    private void UpdateWaveAICount()
    {
        waveAICount = 0;
        foreach (var keyValuePair in waveZombieData)
        {
            Debug.Log($"Current wave will have {keyValuePair.Value} amount of {keyValuePair.Key}");
            waveAICount += keyValuePair.Value;
        }
        Debug.Log($"Current wave:{waveIndex} will have {waveAICount} AI in total");
    }

    private bool IsWaveSpawning()
    {
        for (int i = 0; i < aiSpawners.Count; i++)
        {
            AISpawner spawner = aiSpawners[i];
            if (spawner.GetIsSpawning())
            {
                return true;
            }
        }

        return false;
    }
    
    private bool IsAllAIKilled()
    {
        int totalReaminningZombies = 0;
        for (int i = 0; i < aiSpawners.Count; i++)
        {
            AISpawner spawner = aiSpawners[i];
            int remainningZombies = spawner.GetActiveZombies();
            totalReaminningZombies += remainningZombies;
        } 
        UIManager.Instance.UpdateWaveIndicatorText($"Wave {waveIndex} | {totalReaminningZombies} zombies left"); 
        return totalReaminningZombies == 0;
    }

    void PlayWaveToggleSound()
    {
        playerController.GetComponent<CharacterSoundController>().PlayGenericAnnouncerSound();
    }

}
