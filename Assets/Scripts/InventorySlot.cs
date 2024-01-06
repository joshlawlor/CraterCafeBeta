using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI labelText;
    public TextMeshProUGUI stackSizeText;

    public void ClearSlot()
    {
        icon.enabled = false;
        labelText.enabled = false;
        stackSizeText.enabled = false;
    }

    public void SelectSlot(bool isSelected)
    {
        // Change the slot appearance based on selection
        // For example, you can change the background color or use SlotHighlight
        // Here, I'm changing the background color of the parent GameObject

        // Hexadecimal values for green and black
        string hexBlue = "#69BCD4";
        string hexBlack = "#4B4848";

        Color backgroundColor = isSelected ? HexToColor(hexBlue) : HexToColor(hexBlack);
        GetComponent<Image>().color = backgroundColor;
    }

    // Function to convert hexadecimal string to Color
    Color HexToColor(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        else
        {
            // Handle parsing error
            return Color.white; // Default color if parsing fails
        }
    }
    public void FillSlot(InventoryItem item)
    {
        if (item == null)
        {
            ClearSlot();
            return;
        }

        icon.enabled = true;
        labelText.enabled = true;
        stackSizeText.enabled = true;

        icon.sprite = item.itemData.icon;
        labelText.text = item.itemData.itemName;
        stackSizeText.text = item.stackSize.ToString();
    }
}
