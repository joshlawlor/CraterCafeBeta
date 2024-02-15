using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventoryItem

{

    public ItemData itemData;
    public int stackSize;
    public int itemID;

    public InventoryItem(ItemData item, int itemID)
    {

        itemData = item;
        this.itemID = itemID;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

}
