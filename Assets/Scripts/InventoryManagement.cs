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
    public void HandleSlotAction(int slotIndex)
    {
        // Check if the player is in range of an NPC
        bool playerInRange = CheckPlayerInRange(); // Implement this method based on my  NPC detection logic

        // Get the selected slot
        InventorySlot selectedSlot = inventorySlots[slotIndex];

        // If the player is in range of an NPC, trigger the "GIVE" action
        if (playerInRange)
        {
            GiveAction(selectedSlot);
        }
        else
        {
            // If the player is not in range, trigger the "DROP" action
            DropAction(selectedSlot);
        }
    }

    private bool CheckPlayerInRange()
    {
        // Implement logic to check if the player is in range of an NPC
        // Return true if the player is in range, false otherwise
        return true; // Update this based on your actual logic
    }
    private void GiveAction(InventorySlot selectedSlot)
    {
        // Implement logic for the "GIVE" action here
        // For example, you can access the item from selectedSlot and interact with the NPC
        Debug.Log("GIVE action triggered!");
    }
    private void DropAction(InventorySlot selectedSlot)
    {
        // Implement logic for the "DROP" action here
        // For example, you can remove the item from the inventory and drop it in the game world
        Debug.Log("DROP action triggered!");
    }

}
