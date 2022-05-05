using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<int, InventoryItem> items = new Dictionary<int, InventoryItem>();

    public Transform ItemContent;
    public GameObject InventoryItem2DObject;
    public Transform equppedItemContent;

    public Item EquippedItem;

    PlayerController playerController;
    PlayerStats playerStats;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerStats =playerController.gameObject.GetComponent<PlayerStats>();
    }

    public bool HasItem(int itemId)
    {
        return items.ContainsKey(itemId);
    }
    
    public void AddItem(Item item)
    {
        if (items.ContainsKey(item.id)){
            items[item.id].quantity = items[item.id].quantity + 1;
            items[item.id].weight = items[item.id].weight + item.weight;
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item.id, item, 1, item.weight);
            items.Add(newItem.id, newItem);
        }
        playerStats.SetWeight(GetWeight());
        playerController.UpdateInfoUIIfOpened();
        UpdateUI();
    }

    public void RemoveItem(Item item, bool isDiscard=true)
    {
        if (items.ContainsKey(item.id))
        {
            if (items[item.id].quantity > 1)
            {
                items[item.id].quantity = items[item.id].quantity - 1;
                items[item.id].weight = items[item.id].weight - item.weight;
            }
            else
            {
                if(EquippedItem != null && EquippedItem.id == item.id)
                {
                    
                    EquippedItem = null;
                }
                items.Remove(item.id);
            }
            playerStats.SetWeight(GetWeight());
            playerController.UpdateInfoUIIfOpened();
            UpdateUI();

            if (isDiscard)
            {
                SpawnItem(item);
            }
        }
        else
        {
            Debug.Log("[RemoveError] Undefined removed item");
        }
    }


    public void EquipItem(Item item)
    {
        EquippedItem = item;
        UpdateUI();
    }

    public void SwitchEquippedItem()
    {
        if(items.Count == 0 || (items.Count == 1 && EquippedItem != null))
        {
            return;
        }
        int pos = 0;
        InventoryItem[] inventoryItemsArray = items.Values.ToArray<InventoryItem>();
        for(int i = 0; i < inventoryItemsArray.Length; i++)
        {
            InventoryItem item = inventoryItemsArray[i];
            if(EquippedItem == null)
            {
                EquipItem(item.Item);
                return;
            }
            if (EquippedItem.id == item.id)
            {
                pos = i;
                break;
            }
        }
        Item target = pos < inventoryItemsArray.Length - 1 ? inventoryItemsArray[pos + 1].Item : inventoryItemsArray[0].Item;
        EquipItem(target);
    }

    public void SpawnItem(Item s_item)
    {
        //Debug.Log("Spawn item " + s_item.ItemName);
        GameObject item =  Instantiate(s_item.prefab, playerController.transform.position + playerController.transform.forward*3 + Vector3.up*2, Quaternion.identity);

    }

    public GameObject SpawnItem(GameObject prefab)
    {
        GameObject item =  Instantiate(prefab, playerController.transform.position + playerController.transform.forward*3 + Vector3.up*2, Quaternion.identity);
        return item;
    }

    public void UseItem(Item s_item)
    {
        if (s_item == null)
        {
            Debug.Log("You must use a valid item");
            UIManager.Instance.PushNotification("You must equip a item before use");
            return;
        }
        if (s_item.prefab.GetComponent<IUsable>() != null)
        {
            RemoveItem(s_item, false);
            Debug.Log("Use item " + s_item.ItemName);
            if (s_item.spawnWhenUse)
            {
                GameObject obj = SpawnItem(s_item.prefab);
                obj.GetComponent<IUsable>().Use();
            }
            else
            {
                s_item.prefab.GetComponent<IUsable>().Use();
            }
        }
        else
        {
            Debug.Log($"Item is not a userable item: {s_item.name}");
            UIManager.Instance.PushNotification($"{s_item.ItemName} is not a usable item");
        }
    }

    public void UpdateUI()
    {
        var equippedItemImage = equppedItemContent.transform.Find("Item").GetComponent<Image>();

        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        var defaultLabel = ItemContent.transform.parent.transform.Find("DefaultLabel").gameObject;
        if (items.Count == 0)
        {
            defaultLabel.SetActive(true);
            equippedItemImage.sprite = null;
            return;
        }
        defaultLabel.SetActive(false);

        foreach (var inventoryItem in items)
        {
            Item item = inventoryItem.Value.Item;
            int quantity = inventoryItem.Value.quantity;
            float weight = inventoryItem.Value.weight;
            GameObject obj = Instantiate(InventoryItem2DObject, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemQunatity = obj.transform.Find("ItemCount").GetComponent<Text>();
            var itemWeight = obj.transform.Find("ItemWeight").GetComponent<Text>();
            var itemStatusButton = obj.transform.Find("Status").gameObject;
            itemName.text = item.ItemName;
            itemQunatity.text = "X" + quantity.ToString();
            itemWeight.text = weight.ToString() + " KG";
            itemIcon.sprite = item.icon;
            itemStatusButton.SetActive(EquippedItem != null && EquippedItem.id == item.id);
            // InventoryItemController
            obj.GetComponent<InventoryItemController>().SetItem(item);
        }

        
        equippedItemImage.sprite = EquippedItem != null ? EquippedItem.icon : null;
    }

    public float GetWeight()
    {
        float weight = 0f;
        foreach (var inventoryItem in items)
        {
            Item item = inventoryItem.Value.Item;
            float w = inventoryItem.Value.weight;
            weight += w;
        }
        return weight;
    }


    public void ApplyItemEffect(StatusEffectData effectData)
    {
        playerStats.ApplyEffect(effectData);
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }


}
