using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItemController : MonoBehaviour
{
    public GameObject popUp; 

    private bool playerInRange = false;

    public ItemData itemData;


    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && Inventory.Instance.inventory.Count < 3)
        {
            Inventory inventory = FindObjectOfType<Inventory>();

            if (inventory != null && itemData != null)
            {
                Inventory.Instance.Add(itemData);
                Destroy(gameObject);
            }
            
        }
    }




    // POP UP CODE: //

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Inventory.Instance.inventory.Count < 3)
        {
            playerInRange = true;
            SetPopUpActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            SetPopUpActive(false);
        }
    }

    private void SetPopUpActive(bool isActive)
    {
        if (popUp != null)
        {
            popUp.SetActive(isActive);
        }
    }
}