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
                SetTalkPopUpActive(true);
                eraseText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                SetTalkPopUpActive(false);
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
                    SetTalkPopUpActive(false);
                    SetGivePopUpActive(true);
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
                    GiveItemToNPC();
                    SetGivePopUpActive(false);
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
            SetTalkPopUpActive(true); //
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            SetTalkPopUpActive(false);
            SetGivePopUpActive(false);
            eraseText();
        }
    }

    private void SetTalkPopUpActive(bool isActive)
    {
        if (talkPopUp != null)
        {
            talkPopUp.SetActive(isActive);
        }
    }

    private void SetGivePopUpActive(bool isActive)
    {
        if (givePopUp != null)
        {
            givePopUp.SetActive(isActive);
        }
    }

    private void GiveItemToNPC()
    {
        // Get the selected item from the inventory
        InventoryItem selectedItem = Inventory.Instance.inventory[Inventory.Instance.SelectedIndex];

        Inventory.Instance.Remove(selectedItem.itemData);

        // Log a message (you can replace this with your actual logic)
        Debug.Log($"Gave {selectedItem.itemData.itemName} to NPC!");
    }
}
