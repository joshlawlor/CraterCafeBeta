using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNPCSpawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    public GameObject[] NPCPrefabs;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int randNPC = Random.Range(0, NPCPrefabs.Length);

            Instantiate(NPCPrefabs[randNPC], spawnPoint[0].position, transform.rotation);
        }
    }
}
