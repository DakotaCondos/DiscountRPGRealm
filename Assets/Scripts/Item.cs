using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public string itemName;
    public string itemDescription;
    public int itemValue;
    public List<ItemEffects> itemEffects = new();
    public Texture2D image;
    public bool isConsumable;

    public Item(string itemName, string itemDescription, int itemValue, List<ItemEffects> itemEffects, Texture2D image, bool isConsumable = false)
    {
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemValue = itemValue;
        this.itemEffects = itemEffects;
        this.image = image;
        this.isConsumable = isConsumable;
    }
}
