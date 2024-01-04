using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Singleton instance
    private static Inventory instance;

    // List of items in the inventory
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    // Public property to get the singleton instance
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                // Try to find an existing instance in the scene
                instance = FindObjectOfType<Inventory>();

                // If no instance is found, create a new GameObject with the Inventory script
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("InventoryManager");
                    instance = singletonObject.AddComponent<Inventory>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        // Ensure there's only one instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Add an item to the inventory
    public void Add(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("Trying to add a null item to the inventory.");
            return;
        }

        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToStack();
            Debug.Log($"Added {itemData.itemName} to the inventory. Stack size: {item.stackSize}");
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            Debug.Log($"Added {itemData.itemName} to the inventory for the first time!");
        }

    }

    // Remove an item from the inventory
    public void Remove(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();
            if (item.stackSize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }
    }
}



