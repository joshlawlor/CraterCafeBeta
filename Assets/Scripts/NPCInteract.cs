using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCInteract : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;

    public GameObject continueButton;
    public GameObject popUp; // Reference to the PopUp GameObject

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
                SetPopUpActive(true);
                eraseText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                SetPopUpActive(false);
                StartCoroutine(Typing());
            }
        }

        if (dialogueText.text == dialogue[index])
        {
            continueButton.SetActive(true);
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
            SetPopUpActive(true); //
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            SetPopUpActive(false);
            eraseText();
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
