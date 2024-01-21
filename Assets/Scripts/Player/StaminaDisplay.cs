using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaminaDisplay : MonoBehaviour
{

    public PlayerMovement playerMovement;
    public TextMeshProUGUI staminaText;

    void Start()
    {
        // Ensure the playerMovement reference is set
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement reference not set in StaminaDisplay script!");
        }
    }

    void Update()
    {
        // Update TMP text with the stamina percentage
        UpdateStaminaText();
    }

    void UpdateStaminaText()
    {
        // Ensure the staminaText reference is set
        if (staminaText != null && playerMovement != null)
        {
            // Get the stamina percentage from the PlayerMovement script
            float staminaPercentage = playerMovement.StaminaPercentage;

            // Format the percentage as text
            string formattedText = $"Stamina: {staminaPercentage:P0}";

            // Update the TMP text
            staminaText.text = formattedText;
        }
        else
        {
            Debug.LogError("StaminaText or PlayerMovement not set in StaminaDisplay script!");
        }
    }
}
