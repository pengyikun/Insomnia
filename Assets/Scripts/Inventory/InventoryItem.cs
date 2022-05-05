using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public int id;
    public Item Item;
    public int quantity;
    public float weight;

    public InventoryItem(int n_id, Item n_item, int n_quantity, float n_weight)
    {
        id = n_id;
        Item = n_item;
        quantity = n_quantity;
        weight = n_weight;
    }
}
