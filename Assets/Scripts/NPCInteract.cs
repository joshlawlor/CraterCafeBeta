using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCInteract : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;

    public GameObject continueButton;
    public GameObject talkPopUp; // Reference to the PopUp GameObject
    public GameObject givePopUp;

    public GameObject successPopUp;
    public GameObject failPopUp;
    public GameObject dropPopUp;
    public NPCOrderData orderData;

    public string[] dialogue;

    private int index;
    public float wordSpeed;
    public bool playerIsClose;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {

            if (dialoguePanel.activeInHierarchy)
            {
                SetPopUpActive(talkPopUp, true);
                eraseText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                SetPopUpActive(talkPopUp, false);
                StartCoroutine(Typing());
            }
        }

        if (dialogueText.text == dialogue[index])
        {
            continueButton.SetActive(true);
        }

        if (playerIsClose && Inventory.Instance.SelectedIndex != -1)
        {
            int selectedIndex = Inventory.Instance.SelectedIndex;

            // Check if the selected index is within the valid range of the inventory list
            if (selectedIndex >= 0 && selectedIndex < Inventory.Instance.inventory.Count)
            {
                InventoryItem selectedItem = Inventory.Instance.inventory[selectedIndex];

                // Check if the stack size is greater than 1 before giving the item
                if (selectedItem != null)
                {
                    SetPopUpActive(talkPopUp, false);
                    SetPopUpActive(givePopUp, true);
                }
            }

        }


        // Check for 'G' key press to give item
        if (Input.GetKeyDown(KeyCode.G) && playerIsClose && Inventory.Instance.SelectedIndex != -1)
        {
            int selectedIndex = Inventory.Instance.SelectedIndex;

            // Check if the selected index is within the valid range of the inventory list
            if (selectedIndex >= 0 && selectedIndex < Inventory.Instance.inventory.Count)
            {
                // Get the selected item from the inventory
                InventoryItem selectedItem = Inventory.Instance.inventory[selectedIndex];

                // Check if the stack size is greater than 0 before giving the item
                if (selectedItem.stackSize > 0)
                {
                    GiveItemToNPC(selectedItem.itemData);
                    SetPopUpActive(givePopUp, false);
                }
                else
                {
                    // Handle the case where stack size is not greater than 0 (optional)
                    Debug.Log("Cannot give item with 0");
                }
            }
        }
    }

    public void eraseText()
    {
        if (dialoguePanel)
        {
            dialogueText.text = "";
            dialoguePanel.SetActive(false);
            index = 0;
        }

    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        continueButton.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            eraseText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            ResetAllPopUps();
            SetPopUpActive(talkPopUp, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            ResetAllPopUps();
            eraseText();
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
        SetPopUpActive(talkPopUp, false);
        SetPopUpActive(givePopUp, false);
        SetPopUpActive(dropPopUp, false);
        SetPopUpActive(failPopUp, false);
        SetPopUpActive(successPopUp, false);
    }
    private void GiveItemToNPC(ItemData itemData)
    {
        // Get the selected item from the inventory
        InventoryItem selectedItem = Inventory.Instance.inventory[Inventory.Instance.SelectedIndex];

        Inventory.Instance.Remove(selectedItem.itemData);

        if (orderData != null && itemData.itemName == orderData.orderItemName)
        {
            Debug.Log("Order fulfilled!");
            ResetAllPopUps();
            SetPopUpActive(successPopUp, true);
        }
        else
        {
            Debug.Log("Order failed!");
            ResetAllPopUps();
            SetPopUpActive(failPopUp, true);

        }

        // Log a message (you can replace this with your actual logic)
        Debug.Log($"Gave {selectedItem.itemData.itemName} to NPC!");
    }
}
