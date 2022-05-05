using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> itemPrefabs;
    private GameObject spawnedItem;

    public void Spawn()
    {
        if (!IsEmpty()) return;
        GameObject item =  Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Count)], transform.position + Vector3.up*2, Quaternion.identity);
        spawnedItem = item;
        Debug.Log($"{gameObject.name} spawned {item.name}");
    }

    public void Clear()
    {
        if(spawnedItem != null) Destroy(spawnedItem);
    }

    public bool IsEmpty()
    {
        return spawnedItem == null;
    }
}
