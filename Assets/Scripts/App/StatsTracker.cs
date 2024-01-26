using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsTracker : MonoBehaviour
{
    public TextMeshProUGUI npcCountText; // Reference to the TMP object
    public GameObject ClosedDoors;
    public GameObject OpenDoors;

    private int npcCount; // Store the npcCount locally

    private void Update()
    {
        ActiveNPCTracker();
        if (npcCount == 0)
        {
            Debug.Log("NO ACTIVE NPC");
            OpenDoors.SetActive(false);
            ClosedDoors.SetActive(true);
        }
        else
        {
            OpenDoors.SetActive(true);
            ClosedDoors.SetActive(false);
        }
    }

    private void UpdateTotalNPC(int npcCount)
    {
        this.npcCount = npcCount; // Update the local npcCount
        if (npcCountText != null)
        {
            npcCountText.text = "Customers: " + npcCount.ToString();
        }


    }

    public int GetNPCCount()
    {
        return npcCount; // Return the current npcCount
    }

    private void ActiveNPCTracker()
    {
        // Find all active GameObjects with names starting with 'NPC'
        GameObject[] activeNPCs = GameObject.FindGameObjectsWithTag("NPC");
        UpdateTotalNPC(activeNPCs.Length);
        // You can now iterate through the array of active NPCs and perform any desired actions
        foreach (GameObject npc in activeNPCs)
        {
            // Example: Output the names of active NPCs
            Debug.Log("Active NPC: " + npc.name);
        }
    }
}

