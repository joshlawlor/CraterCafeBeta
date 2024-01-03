using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public string description;

    public int itemCost;
}
