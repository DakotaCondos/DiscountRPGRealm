using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public string itemDescription = "";
    public int itemValue;
    public List<ItemEffects> itemEffects = new();
    public Texture2D image;
    public bool isConsumable;
}
