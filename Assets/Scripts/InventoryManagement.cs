using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InventoryManagement : MonoBehaviour
{
    public GameObject slotPrefab;
    public List<InventorySlot> inventorySlots = new List<InventorySlot>(11);

    private void OnEnable()
    {
        Inventory.Instance.OnInventoryChange += FillInventory;
    }

    private void OnDisable()
    {
        Inventory.Instance.OnInventoryChange -= FillInventory;
    }


    private void Update()
    {
        // Handle slot selection based on player input (1-5)
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Inventory.Instance.SelectedIndex = i;
            }
        }

        // Update slot appearance based on selection
        UpdateSlotSelection();
    }

    void ResetInventory()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
        inventorySlots = new List<InventorySlot>(11);
    }

    void FillInventory(List<InventoryItem> inventory)
    {
        ResetInventory();

        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            //Create 11 Slots
            CreateInventorySlot();

        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventorySlots[i].FillSlot(inventory[i]);
        }

    }

    void CreateInventorySlot()
    {
        // Check if the inventorySlotPrefab is not null before instantiating
        if (slotPrefab != null)
        {
            GameObject newSlot = Instantiate(slotPrefab);
            newSlot.transform.SetParent(transform, false);

            InventorySlot newSlotComponent = newSlot.GetComponent<InventorySlot>();
            newSlotComponent.ClearSlot();

            inventorySlots.Add(newSlotComponent);
        }
        else
        {
            Debug.LogError("Inventory slot prefab is null. Please assign a prefab in the inspector.");
        }

    }
    private void UpdateSlotSelection()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].SelectSlot(i == Inventory.Instance.SelectedIndex);
        }
    }   

}
