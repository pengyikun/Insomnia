using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryItemController : MonoBehaviour
{
    Item item;

    public Button RemoveButton;

    public void RemoveItem()
    {
        InventoryManager.Instance.RemoveItem(item);
        // Destroy(gameObject);
    }

    public void EquipItem()
    {
        InventoryManager.Instance.EquipItem(item);
    }

    public void SetItem(Item n_item)
    {
        item = n_item;
    }
}
