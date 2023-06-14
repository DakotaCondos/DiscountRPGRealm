using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public string itemName;
    public int itemValue;
    public List<ItemEffects> itemEffects = new();
    public Texture2D image;
    public bool isConsumable;

    public Item(string itemName, int itemValue, List<ItemEffects> itemEffects, Texture2D image, bool isConsumable = false)
    {
        this.itemName = itemName;
        this.itemValue = itemValue;
        this.itemEffects = itemEffects;
        this.image = image;
        this.isConsumable = isConsumable;
    }
}
