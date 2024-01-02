using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItemController : MonoBehaviour
{
    public GameObject popUp; // Reference to the PopUp GameObject

    private bool playerInRange = false;

    private void Update()
    {
        // Check if the player is in range and presses the "F" key
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            // Set the PopUp GameObject to active
            SetPopUpActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            // Set the PopUp GameObject to inactive
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