using System;
using System.Collections;
using System.Collections.Generic;
using EmeraldAI.Utility;
using UnityEngine;

public class AISpawnerLocative : MonoBehaviour
{
    public GameObject aiPrefab;
    public float startWaitInterval;
    public float spawnInterval;
    public int maxCount;
    private List<GameObject> spawnedAIs = new List<GameObject>();

    private void Start()
    {
        StartCoroutine("WaitSpawner");
    }

    IEnumerator WaitSpawner()
    {
        yield return new WaitForSeconds(startWaitInterval);
        while (spawnedAIs.Count < maxCount)
        {
                    GameObject ai = EmeraldAIObjectPool.Spawn(aiPrefab, transform.position, Quaternion.identity);
                    spawnedAIs.Add(ai);
                    yield return new WaitForSeconds(spawnInterval);
        }

    }
    
}
