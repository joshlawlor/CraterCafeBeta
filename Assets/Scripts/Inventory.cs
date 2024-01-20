using System;
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
            if (instance == null && Application.isPlaying)
            {
                instance = FindObjectOfType<Inventory>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("InventoryManager");
                    instance = singletonObject.AddComponent<Inventory>();
                    DontDestroyOnLoad(singletonObject);  // Prevent destruction when loading a new scene

                }
            }

            return instance;
        }
    }

    public event Action<List<InventoryItem>> OnInventoryChange;

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

        if (inventory.Count >= 3)
        {
            Debug.Log("Inventory is full. Cannot add more items.");
            return;
        }

        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            // item.AddToStack();
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            Debug.Log($"Added {itemData.itemName} to the inventory. Stack size: {item.stackSize}");
            OnInventoryChange?.Invoke(inventory);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            Debug.Log($"Added {itemData.itemName} to the inventory for the first time!");
            OnInventoryChange?.Invoke(inventory);
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
                OnInventoryChange?.Invoke(inventory);
            }
        }
    }


    private int selectedIndex = 0;

    public int SelectedIndex
    {
        get { return selectedIndex; }
        set
        {
            if (value >= 0 && value < inventory.Count)
            {
                selectedIndex = value;
                OnInventoryChange?.Invoke(inventory);
            }
        }
    }
    private void OnDestroy()
    {
        // Make sure to set the instance to null when the object is destroyed
        instance = null;
    }


}



