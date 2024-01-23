using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNPCSpawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    public GameObject[] NPCPrefabs;

    public float spawnIntervalMin = 5f;
    public float spawnIntervalMax = 10f;
    public int maxNPCs = 5;

    private void Start()
    {

        StartCoroutine(SpawnNPCs());

    }

      private IEnumerator SpawnNPCs()
    {
        int spawnedNPCs = 0;

        // Continue spawning NPCs until the maximum limit is reached
        while (spawnedNPCs < maxNPCs)
        {
            // Wait for a random interval between spawnIntervalMin and spawnIntervalMax
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            // Randomly choose an NPC prefab
            int randNPC = Random.Range(0, NPCPrefabs.Length);

            // Instantiate the NPC at the chosen spawn point
            Instantiate(NPCPrefabs[randNPC], spawnPoint[0].position, Quaternion.identity);

            // Increment the count of spawned NPCs
            spawnedNPCs++;
        }
    }
}
