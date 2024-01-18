using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTapController : MonoBehaviour
{
    public GameObject InteractPopUp;
    private bool playerInRange = false;

    public ItemData itemData;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && Inventory.Instance.inventory.Count < 3)
        {
            Inventory inventory = FindObjectOfType<Inventory>();

            if (inventory != null && itemData != null)
            {
                Inventory.Instance.Add(itemData);
            }

        }
    }



    private void SetPopUpActive(GameObject popUp, bool isActive)
    {
        if (popUp != null)
        {
            popUp.SetActive(isActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            SetPopUpActive(InteractPopUp, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            SetPopUpActive(InteractPopUp, false);

        }
    }

}
