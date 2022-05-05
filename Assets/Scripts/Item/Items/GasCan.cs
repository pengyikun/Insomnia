using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCan : MonoBehaviour, ICollectable 
{
    public Item _Item;
    public Item Item => _Item;


    public void OnInteract()
    {
        Debug.Log($"On interact: {gameObject.name}");
        InventoryManager.Instance.AddItem(_Item);
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnInteract(); 
        }
    }
}
