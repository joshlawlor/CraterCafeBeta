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
        Color backgroundColor = isSelected ? Color.green : Color.black;
        GetComponent<Image>().color = backgroundColor;
    }

    public void FillSlot(InventoryItem item)
    {
        if(item == null)
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
