using System.Collections;
using System.Collections.Generic;
using EmeraldAI;
using UnityEngine;
using EmeraldAI.Utility;

public class AISpawner : MonoBehaviour
{
    public int waveMaxCount = 20;
    public Dictionary<string, GameObject> zombieTypes = new Dictionary<string, GameObject>(); 
    public  Dictionary<string, int> waveZombieData = new Dictionary<string, int>();
    public List<string> waveZombieKeys = new List<string>(); 
    public float startWait;
    public float spawnWait;
    
    private List<GameObject> spawnedAIs = new List<GameObject>();
    private WaveManager waveManager;
    private bool isSpawning = false;

    void Start()
    {
        waveManager = WaveManager.Instance;
    }
    
    public bool GetIsSpawning()
    {
        return isSpawning;
    }

    public void StartSpanwing()
    {
        isSpawning = true;
        Debug.Log($"[{gameObject.name}] Started spawning {waveMaxCount} AIs"); 
        // foreach (var data in waveZombieData)
        // {
        //     Debug.Log($"[{gameObject.name}] Planned to spawn {data.Value} amount of {data.Key}");  
        // }
        StartCoroutine("WaitSpawner");
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void ClearZombies()
    {
        spawnedAIs.ForEach(ai =>
        {
            // spawnedAIs.Remove(ai);
            if(ai != null) Destroy(ai);
        });
        spawnedAIs = new List<GameObject>();
    }

    IEnumerator WaitSpawner()
    {
        yield return new WaitForSeconds(startWait);
        while (spawnedAIs.Count < waveMaxCount && isSpawning)
        {
            for (int i = 0; i < waveZombieKeys.Count; i++)
            {
                //Debug.Log($"[{gameObject.name}] {waveZombieData[waveZombieKeys[i]]} of {waveZombieKeys[i]} left");  
                if (waveZombieData[waveZombieKeys[i]] > 0)
                {
                    //Debug.Log($"[{gameObject.name}] planned to spawn {waveZombieKeys[i]}");
                    GameObject ai = EmeraldAIObjectPool.Spawn(zombieTypes[waveZombieKeys[i]],
                        transform.position, Quaternion.identity);
                    waveZombieData[waveZombieKeys[i]] = waveZombieData[waveZombieKeys[i]] - 1;
                    //Debug.Log($"[{gameObject.name}] Spawned {waveZombieKeys[i]}");
                    spawnedAIs.Add(ai);
                    break;
                }
            }

            //Debug.Log("Go next");
            //spawnedAIs.Add(SpawnAvaliableAI());
            yield return new WaitForSeconds(spawnWait);
        }
        StopSpawning();
        Debug.Log($"[{gameObject.name}] Finished spawning for current wave");
    }

    public void UpdateWaveData(Dictionary<string, int> data)
    {
        ClearZombies();
        waveZombieData = new Dictionary<string, int>();
        waveZombieKeys = new List<string>();
        waveMaxCount = 0;
        foreach (var d in data)
        {
            waveZombieData.Add(d.Key, d.Value);
            waveZombieKeys.Add(d.Key);
            waveMaxCount += data[d.Key];
        }

    }

    public bool IsAllAIKilled()
    {
        for (int i = 0; i < spawnedAIs.Count; i++)
        {
            GameObject ai = spawnedAIs[i];
            if (!ai.GetComponent<EmeraldAISystem>().IsDead)
            {
                return false;
            }
        }
        return true;
    }

    public int GetActiveZombies()
    {
        int num = 0;
        for (int i = 0; i < spawnedAIs.Count; i++)
        {
            GameObject ai = spawnedAIs[i];
            if (!ai.GetComponent<EmeraldAISystem>().IsDead)
            {
                num += 1;
            }
        }
        return num; 
    }

    // GameObject SpawnAvaliableAI()
    // {
    //     
    //     // Debug.Log(waveZombieKeys.Count);
    //     // string zombieType = waveZombieKeys[Random.Range(0, waveZombieKeys.Count)]; 
    //     // int aiSpawnedLeft = waveZombieData[zombieType];
    //     // if (aiSpawnedLeft > 0)
    //     // {
    //     //     GameObject ai = EmeraldAIObjectPool.Spawn(zombieTypes[zombieType],
    //     //         transform.position, Quaternion.identity);
    //     //     waveZombieData[zombieType] = waveZombieData[zombieType] - 1;
    //     //     Debug.Log($"[{gameObject.name}] Spawned {zombieType}");
    //     //     return ai;
    //     // }
    //     // return SpawnAvaliableAI();
    // }
}
