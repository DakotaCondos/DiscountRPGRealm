using Nova;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemRow : MonoBehaviour
{
    [Header("Item UI")]
    public GameObject itemEffectPrefab;
    public Transform itemEffectsLocation;
    public List<GameObject> itemEffectsUI = new();
    public TextBlock itemNameTextBlock;
    public UIBlock2D itemImageBlock;

    [Header("Item")]
    public Item item;

    [Header("Button")]
    public TextBlock buttonTextBlock;
    public Action<Item> buttonAction;

    public void Display(Item item)
    {
        this.item = item;
        itemNameTextBlock.Text = item.itemName;
        itemImageBlock.SetImage(item.image);
        BuildItemEffects(item);
        SetButtonText();
    }

    private void SetButtonText()
    {
        if (item.isConsumable)
        {
            buttonTextBlock.Text = "Use";
        }
        else
        {
            Item equipped = TurnManager.Instance.GetCurrentActor().player.equippedItem;
            if (equipped != null && equipped.Equals(item))
            {
                buttonTextBlock.Text = "Unequip";
            }
            else
            {
                buttonTextBlock.Text = "Equip";
            }
        }
    }

    private void BuildItemEffects(Item item)
    {
        UIReferences uiRef = UIReferences.Instance;

        foreach (GameObject g in itemEffectsUI)
        {
            Destroy(g);
        }
        itemEffectsUI.Clear();
        foreach (ItemEffects effect in item.itemEffects)
        {
            if (effect.value == 0) { continue; }

            Texture2D icon = effect.Type switch
            {
                ItemEffectType.Power => uiRef.powerIcon,
                ItemEffectType.PowerVsPlayer => uiRef.powerPlayerIcon,
                ItemEffectType.PowerVsMonster => uiRef.powerMonsterIcon,
                ItemEffectType.Movement => uiRef.movementIcon,
                ItemEffectType.Teleport => uiRef.teleportIcon,
                _ => uiRef.unknownIcon,
            };

            GameObject g = Instantiate(itemEffectPrefab, itemEffectsLocation);
            itemEffectsUI.Add(g);

            UIHelper h = g.GetComponent<UIHelper>();
            h.UIBlock2Ds[0].SetImage(icon);
            h.TextBlocks[0].Text = (effect.value == 0) ? "???" : (effect.value > 0) ? $"+{effect.value}" : effect.value.ToString();
        }
    }

    public void SelectItem()
    {
        if (buttonAction is null)
        {
            Debug.LogWarning("buttonAction is not assigned");
            return;
        }
        buttonAction(item);
    }
}