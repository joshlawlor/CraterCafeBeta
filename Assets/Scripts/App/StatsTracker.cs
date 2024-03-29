using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsTracker : MonoBehaviour
{
    public TextMeshProUGUI npcCountText;
    public TextMeshProUGUI totalNPCText;
    public GameObject ClosedDoors;
    public GameObject OpenDoors;

    private int npcCount; // Store the active npcCount locally
    private int totalSpawnedNPCs; // Store the total spawned NPC count

    private void Update()
    {
        ActiveNPCTracker();
        UpdateTotalDisplay();
        if (npcCount == 0)
        {
            if (OpenDoors != null && ClosedDoors != null)
            {
                OpenDoors.SetActive(false);
                ClosedDoors.SetActive(true);
            }

        }
        else
        {
            if (OpenDoors != null && ClosedDoors != null)
            {
                OpenDoors.SetActive(true);
                ClosedDoors.SetActive(false);
            }
        }
    }

    public void UpdateTotalNPC()
    {
        totalSpawnedNPCs++;
        UpdateTotalDisplay();
    }

    private void UpdateTotalDisplay()
    {
        if (totalNPCText != null)
        {
            totalNPCText.text = "Total Customers: " + GetTotalSpawnedNPCs();
        }
    }

    private void UpdateTotalActiveNPC(int npcCount)
    {
        this.npcCount = npcCount; // Update the active npcCount
        if (npcCountText != null)
        {
            npcCountText.text = "Active Customers: " + npcCount.ToString();
        }
    }

    public int GetNPCCount()
    {
        return npcCount; // Return the current npcCount
    }

    public int GetTotalSpawnedNPCs()
    {
        return totalSpawnedNPCs; // Return the total spawned NPC count
    }

    public void SetTotalSpawnedNPCs(int npcCount)
    {
        totalSpawnedNPCs = npcCount;
    }

    private void ActiveNPCTracker()
    {
        // Find all active GameObjects with names starting with 'NPC'
        GameObject[] activeNPCs = GameObject.FindGameObjectsWithTag("NPC");
        int currentNPCCount = activeNPCs.Length;

        // Check if the npcCount has changed
        if (currentNPCCount != npcCount)
        {
            UpdateTotalNPC();
        }

        // Update the active NPC count
        UpdateTotalActiveNPC(currentNPCCount);
    }
}
