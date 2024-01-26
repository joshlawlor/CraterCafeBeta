using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StartGame : MonoBehaviour
{
    public TextMeshProUGUI BarStatus;

    public StatsTracker statsTracker; // Reference to the StatsTracker script

    public RandomNPCSpawner randomNPCSpawning;

    public GameObject OpenPopUp;
    public GameObject OpenDoors;

    public GameObject ClosedPopUp;

    public GameObject ClosedDoors;


    private bool barOpen = false;
    private bool playerInRange = false;

    // Called when another collider enters this collider's trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            // Player has entered the trigger zone
            playerInRange = true;

            if (barOpen == false)
            {
                SetPopUpActive(OpenPopUp, true);
            }
            else if (barOpen == true)
            {
                SetPopUpActive(ClosedPopUp, true);

            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player has exited the trigger zone
            playerInRange = false;
            ResetAllPopUps();
            Debug.Log("Player exited the trigger zone.");
        }



    }

    private void UpdateBarStatus(bool barOpen)
    {
        this.barOpen = barOpen; // Update the local npcCount
        if (BarStatus != null)
        {
            if (barOpen == true)
            {
                BarStatus.text = "Bar: Open ";

            }
            else
            {
                BarStatus.text = "Bar: Closed ";

            }
        }

    }





    // Update is called once per frame
    void Update()
    {

        // Check if the player has pressed the space key
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            // Toggle the active status of RandomNPCSpawning
            if (randomNPCSpawning != null)
            {

                if (barOpen == false) // BAR IS CLOSED, THEN OPEN THE BAR
                {
                    OpenDoors.SetActive(!OpenDoors.activeSelf);
                    ClosedDoors.SetActive(!ClosedDoors.activeSelf);
                    randomNPCSpawning.StartSpawning();

                    barOpen = true;
                    UpdateBarStatus(barOpen);
                    ResetAllPopUps();
                    // Output a message indicating the status change
                    Debug.Log("RandomNPCSpawning status toggled. New status: Running");
                }
                else if (barOpen == true) // BAR IS OPEN, THEN CLOSE THE BAR
                {
                    randomNPCSpawning.StopSpawning();
                    barOpen = false;
                    UpdateBarStatus(barOpen);
                    ResetAllPopUps();
                    // Output a message indicating the status change
                    Debug.Log("RandomNPCSpawning status toggled. New status: Stopped ");
                }

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

    private void ResetAllPopUps()
    {
        SetPopUpActive(OpenPopUp, false);
        SetPopUpActive(ClosedPopUp, false);

    }


}
