using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("General")]
    public Texture2D defaultTexture;

    [Header("Player")]
    public TextBlock playerNameTextBlock;
    public UIBlock2D playerBackingBlock;
    public UIBlock2D playerImageBlock;

    [Header("Stats")]
    public TextBlock powerTextBlock;
    public TextBlock movementTextBlock;
    public TextBlock pvmBonusTextBlock;
    public TextBlock pvpBonusTextBlock;

    [Header("Equipped Item")]
    public GameObject equippedItemEffectPrefab;
    public Transform equippedItemEffectsLocation;
    public List<GameObject> equippedItemEffectsUI = new();
    public TextBlock equippedItemNameTextBlock;
    public TextBlock equippedItemDescriptionTextBlock;
    public UIBlock2D equippedItemImageBlock;

    [Header("Inventory Items")]
    public GameObject inventoryItemRowPrefab;
    public Transform inventoryItemsLocation;
    public List<GameObject> inventoryItemsUI = new();


    public static InventoryManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        BuildUI(TurnManager.Instance.GetCurrentActor().player);
    }


    private void OnDisable()
    {
        BuildUI();
    }
    private void BuildUI(Player player = null)
    {
        if (player is null)
        {
            // Reset the Equipped item block
            EquipItem();

            foreach (var item in inventoryItemsUI)
            {
                Destroy(item);
            }
            inventoryItemsUI.Clear();
        }
        else
        {
            if (player.equippedItem != null) { EquipItem(player.equippedItem); }

            // Sort Items
            player.items.Sort((item1, item2) => item1.itemName.CompareTo(item2.itemName));

            foreach (Item item in player.items)
            {
                CreateInventoryItemRow(item);
            }
        }
    }

    private void CreateInventoryItemRow(Item item)
    {
        GameObject g = Instantiate(inventoryItemRowPrefab, inventoryItemsLocation);
        inventoryItemsUI.Add(g);

        g.GetComponent<InventoryItemRow>().Display(item);
    }

    private void EquipItem(Item equipItem = null)
    {
        UIReferences uiRef = UIReferences.Instance;
        if (equipItem is null)
        {
            equippedItemNameTextBlock.Text = "No Item Equipped";
            equippedItemImageBlock.SetImage(uiRef.unknownIcon);
            equippedItemDescriptionTextBlock.Text = "";
            foreach (GameObject item in equippedItemEffectsUI)
            {
                Destroy(item);
            }
            equippedItemEffectsUI.Clear();
            return;
        }
        equippedItemNameTextBlock.Text = equipItem.itemName;
        equippedItemImageBlock.SetImage(equipItem.image);
        equippedItemDescriptionTextBlock.Text = equipItem.itemDescription;

        foreach (ItemEffects effect in equipItem.itemEffects)
        {
            Texture2D icon = effect.Type switch
            {
                ItemEffectType.Power => uiRef.powerIcon,
                ItemEffectType.PowerVsPlayer => uiRef.powerPlayerIcon,
                ItemEffectType.PowerVsMonster => uiRef.powerMonsterIcon,
                ItemEffectType.Movement => uiRef.movementIcon,
                ItemEffectType.Teleport => uiRef.teleportIcon,
                _ => uiRef.unknownIcon,
            };

            GameObject g = Instantiate(equippedItemEffectPrefab, equippedItemEffectsLocation);
            UIHelper h = g.GetComponent<UIHelper>();
            h.UIBlock2Ds[0].SetImage(icon);
            h.TextBlocks[0].Text = (effect.value == 0) ? "" : (effect.value > 0) ? $"+{effect.value}" : effect.value.ToString();

            equippedItemEffectsUI.Add(g);
        }
    }

    public void SelectedInventoryItem(Item itemSelected)
    {
        // Print for now
        ConsolePrinter.PrintToConsole($"Selected inventory item {itemSelected.itemName}", Color.yellow);
    }
}