using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItemController : MonoBehaviour
{
    public GameObject popUp; 

    private bool playerInRange = false;


    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Destroy(gameObject);
        }
    }




    // POP UP CODE: //

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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