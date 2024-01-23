using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPCInteract : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;

    public Image orderIconImage;
    public TextMeshProUGUI orderTimerText;



    public GameObject continueButton;
    public GameObject talkPopUp; // Reference to the PopUp GameObject
    public GameObject givePopUp;

    public GameObject orderPopUp;

    public GameObject successPopUp;
    public GameObject failPopUp;
    public GameObject dropPopUp;
    public NPCOrderData orderData;
    private NPCWander npcWander;
    public Collider2D npcCollider;


    public Animator animator;

    public string[] dialogue;

    private int index;
    public float wordSpeed;
    public bool playerIsClose;
    private bool orderActive = true;
    public float exitTimerDuration = 30f; // Duration of the exit timer in seconds
    public Vector3 exitPoint; // The position where the NPC will move after the timer
    private bool exitTimerActive = false; // Flag to check if the exit timer is active


    private void Start()
    {

        npcWander = GetComponent<NPCWander>();

        // Disable NPCWander script initially
        npcWander.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {

            if (dialoguePanel.activeInHierarchy && orderActive)
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

        if (index < dialogue.Length && dialogueText.text == dialogue[index])
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
                if (selectedItem != null && orderActive)
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
                if (selectedItem.stackSize > 0 && orderActive)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }

        if (exitTimerActive)
        {
            // Check if the exit timer has finished
            if (exitTimerDuration <= 0)
            {
                // Move towards the exit point
                MoveTowardsExitPoint();
            }
            else
            {
                // Update and display the exit timer
                exitTimerDuration -= Time.deltaTime;
                // Update UI or other components if needed
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
        if (other.CompareTag("Player") && orderActive)
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
            if (orderActive)
            {
                SetPopUpActive(talkPopUp, true);

            }
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

    }
    private void GiveItemToNPC(ItemData itemData)
    {
        // Get the selected item from the inventory
        InventoryItem selectedItem = Inventory.Instance.inventory[Inventory.Instance.SelectedIndex];

        Inventory.Instance.Remove(selectedItem.itemData);

        if (orderData != null && itemData.itemName == orderData.orderItemName)
        {
            Debug.Log("Order fulfilled!");
            orderActive = false;
            StartCoroutine(ExitTimer());
            animator.SetBool("OrderSuccess", true);
            ResetAllPopUps();
            SetPopUpActive(successPopUp, true);
            FindObjectOfType<BankScoreController>()?.UpdateBankScore(itemData.itemCost);
            orderTimer = 15f;
        }
        else
        {
            Debug.Log("Order failed!");
            orderActive = false;
            StartCoroutine(ExitTimer());
            animator.SetBool("OrderFail", true);
            ResetAllPopUps();
            SetPopUpActive(failPopUp, true);

        }

        // Log a message (you can replace this with your actual logic)
        Debug.Log($"Gave {selectedItem.itemData.itemName} to NPC!");
    }


    //ORDER LOGIC

    private float orderTimer = 15f;

    public void EnableOrderPopUp()
    {
        // Enable the "order" popUp
        SetPopUpActive(orderPopUp, true);

        // Set the order icon sprite
        if (orderIconImage != null && orderData != null)
        {
            orderIconImage.sprite = orderData.icon;
        }


        // Start the 15-second timer
        StartCoroutine(OrderTimer());
        StartCoroutine(WaitAndEnableWander());

    }

    private IEnumerator OrderTimer()
    {
        while (orderTimer > 0)
        {
            if (orderTimerText != null)
            {
                orderTimerText.text = orderTimer.ToString();
            }
            yield return new WaitForSeconds(1f);
            orderTimer--;
            // Debug.Log("Order timer:" + orderTimer);
            // You can update a timer UI here if needed
        }

        if (orderActive)
        {
            Debug.Log("Order Failed");
            animator.SetBool("OrderFail", true);
            orderActive = false;
            StartCoroutine(ExitTimer());
            ResetAllPopUps();
            SetPopUpActive(failPopUp, true); //
        }

    }

    private IEnumerator ExitTimer()
    {
        // Set the exit timer duration
        exitTimerDuration = 15;

        // Set the exit point (you can set this value wherever needed)
        // exitPoint = new Vector3(10f, 0f, 0f);

        // Set the exit timer active flag
        exitTimerActive = true;

        yield return null;
    }

    private void MoveTowardsExitPoint()
    {
        npcWander.enabled = false;
        npcCollider.enabled = false;

        // Move the NPC towards the exit point
        transform.position = Vector3.MoveTowards(transform.position, exitPoint, 3 * Time.deltaTime);
        Vector3 moveDirection = (exitPoint - transform.position).normalized;

        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.y);
        animator.SetFloat("Speed", moveDirection.magnitude);

        // Check if the NPC has reached the exit point
        if (transform.position == exitPoint)
        {
            // Destroy the NPC GameObject
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitAndEnableWander()
    {
        yield return new WaitForSeconds(2f);

        // Enable NPCWander after waiting for 4 seconds
        npcWander.enabled = true;
    }
}
