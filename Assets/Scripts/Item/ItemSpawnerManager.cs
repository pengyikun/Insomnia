using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ItemSpawnerManager : MonoBehaviour
{
    public static ItemSpawnerManager Instance;
    public float startTime;
    public float spawnInterval;
    private List<ItemSpawner> spawners;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        spawners = Object.FindObjectsOfType<ItemSpawner>().ToList();
        Debug.Log($"[ItemSpawnManager] Detected {spawners.Count} spawners");
        InvokeRepeating("SpawnItems", startTime, spawnInterval);
    }

    public void SpawnItems()
    {
        Debug.Log("Spawnning items in all spawn points");
        foreach (var itemSpawner in spawners)
        {
            itemSpawner.Spawn();
        }
    }
}
