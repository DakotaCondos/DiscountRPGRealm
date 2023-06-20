using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : SceneSingleton<InventoryManager>
{
    [Header("General")]
    public Texture2D defaultTexture;

    [Header("Player")]
    public TextBlock playerNameTextBlock;
    public UIBlock2D playerBackingBlock;
    public UIBlock2D playerImageBlock;
    public Player currentPlayerInventory;
    public TextBlock xpTextBlock;
    public TextBlock lvlTextBlock;
    public TextBlock moneyTextBlock;

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


    private new void Awake()
    {
        base.Awake();
        ResetUI();
    }

    private void OnEnable()
    {
        BuildUI(TurnManager.Instance.GetCurrentActor().player);
    }


    private void OnDisable()
    {
        ResetUI();
    }

    private void ResetUI()
    {
        currentPlayerInventory = null;

        // Reset the Equipped item block
        SetEquippedItem();

        foreach (var item in inventoryItemsUI)
        {
            Destroy(item);
        }
        inventoryItemsUI.Clear();
    }

    private void BuildUI(Player player)
    {
        currentPlayerInventory = player;

        // Sort Items
        player.items.Sort((item1, item2) => item1.itemName.CompareTo(item2.itemName));


        foreach (Item item in player.items)
        {
            CreateInventoryItemRow(item);
        }

        if (player.equippedItem != null)
        {
            EquipItem(player.equippedItem);
        }

        UpdateDisplayDetails(currentPlayerInventory);
    }

    private void UpdateDisplayDetails(Player player)
    {
        // Player
        playerBackingBlock.Color = player.playerColor;
        playerImageBlock.SetImage(player.playerTexture);
        playerNameTextBlock.Text = player.PlayerName;

        // Stats
        powerTextBlock.Text = player.GetPower().ToString();
        movementTextBlock.Text = player.GetMovement().ToString();
        pvmBonusTextBlock.Text = player.powerBonusItemsVsMonster.ToString();
        pvpBonusTextBlock.Text = player.powerBonusItemsVsMonster.ToString();

        // Money Xp Lvl
        moneyTextBlock.Text = player.money.ToString();
        lvlTextBlock.Text = $"Level: {player.level}";
        xpTextBlock.Text = $"Total XP: {player.xp}";
    }

    private void CreateInventoryItemRow(Item item)
    {
        GameObject g = Instantiate(inventoryItemRowPrefab, inventoryItemsLocation);
        inventoryItemsUI.Add(g);

        var j = g.GetComponent<InventoryItemRow>();
        j.Display(item);
        if (j.item.isConsumable)
        {
            j.buttonAction = UseItem;
            j.buttonTextBlock.Text = "Use";
        }
        else
        {
            j.buttonAction = EquipItem;
            j.buttonTextBlock.Text = "Equip";
        }
    }

    private void SetEquippedItem(Item equipItem = null)
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

            GameObject g = Instantiate(equippedItemEffectPrefab, equippedItemEffectsLocation);
            UIHelper h = g.GetComponent<UIHelper>();
            h.UIBlock2Ds[0].SetImage(icon);
            h.TextBlocks[0].Text = (effect.value > 0) ? $"+{effect.value}" : effect.value.ToString();

            equippedItemEffectsUI.Add(g);
        }
    }

    public void EquipItem(Item item)
    {
        if (currentPlayerInventory.equippedItem != null)
        {
            UnequipItem(currentPlayerInventory.equippedItem);
        }

        currentPlayerInventory.equippedItem = item;
        SetEquippedItem(item);

        foreach (GameObject g in inventoryItemsUI)
        {
            var j = g.GetComponent<InventoryItemRow>();
            if (j.item.itemName.Equals(item.itemName))
            {
                j.buttonAction = UnequipItem;
                j.buttonTextBlock.Text = "Unequip";
                break;
            }
        }

        ModifyStats(item);
        UpdateDisplayDetails(currentPlayerInventory);
    }

    public void UnequipItem(Item item)
    {
        currentPlayerInventory.equippedItem = null;
        SetEquippedItem(null);

        // Update InventoryItemRow
        foreach (GameObject g in inventoryItemsUI)
        {
            var j = g.GetComponent<InventoryItemRow>();
            if (j.item.isConsumable) { continue; }
            j.buttonAction = EquipItem;
            j.buttonTextBlock.Text = "Equip";
        }

        // update player stat values
        ModifyStats(null);
        UpdateDisplayDetails(currentPlayerInventory);
    }

    private void ModifyStats(Item item = null)
    {
        if (item == null)
        {
            currentPlayerInventory.powerBonusItems = 0;
            currentPlayerInventory.powerBonusItemsVsMonster = 0;
            currentPlayerInventory.powerBonusItemsVsPlayer = 0;
            currentPlayerInventory.movementBonusItems = 0;
            return;
        }

        foreach (ItemEffects effect in item.itemEffects)
        {
            switch (effect.Type)
            {
                case ItemEffectType.Power:
                    currentPlayerInventory.powerBonusItems = effect.value;
                    break;
                case ItemEffectType.PowerVsPlayer:
                    currentPlayerInventory.powerBonusItemsVsPlayer = effect.value;
                    break;
                case ItemEffectType.PowerVsMonster:
                    currentPlayerInventory.powerBonusItemsVsMonster = effect.value;
                    break;
                case ItemEffectType.Movement:
                    currentPlayerInventory.movementBonusItems = effect.value;
                    break;
                case ItemEffectType.Teleport:
                    Debug.LogWarning("Telepor Effects not implemented");
                    break;
                default:
                    break;
            }
        }

    }

    public void UseItem(Item item)
    {
        // Print for now
        ConsolePrinter.PrintToConsole($"Selected UseItem {item.itemName}", Color.yellow);
    }
}