using Nova;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : SceneSingleton<ShopManager>
{
    [Header("General")]
    public GameObject shopItemRowPrefab;
    public Transform buyRowLocation;
    public Transform sellRowLocation;
    public List<GameObject> buyItemRows = new();
    public List<GameObject> sellItemRows = new();
    public TextBlock playerMoneyTextBlock;
    public Player playerInShop = null;

    [Header("Shop Items")]
    public List<ItemSO> lvl1Items = new();
    public List<ItemSO> lvl2Items = new();
    public List<ItemSO> lvl3Items = new();
    public List<ItemSO> lvl4Items = new();
    public List<ItemSO> lvl5Items = new();
    public List<ItemSO>[] levelsItems;

    private new void Awake()
    {
        base.Awake();
        levelsItems = new List<ItemSO>[] { lvl1Items, lvl2Items, lvl3Items, lvl4Items, lvl5Items };
    }

    private void OnEnable()
    {
        playerInShop = TurnManager.Instance.GetCurrentActor().player;
        BuildShop(playerInShop.currentSpace.shopLevel);
        CalculateMoney();
    }


    private void OnDisable()
    {
        ResetShop();
    }

    private void CalculateMoney()
    {
        playerMoneyTextBlock.Text = playerInShop.money.ToString();
    }

    private void BuildShop(int shopLevel)
    {
        // minimul level for a shop is 1.
        if (shopLevel < 1) { return; }

        for (int i = 0; i < shopLevel; i++)
        {
            DisplayItems(levelsItems[i], true);
        }

        foreach (var item in playerInShop.items)
        {
            CreateItemRow(item, false);
        }

    }

    private void ResetShop()
    {
        foreach (var item in buyItemRows)
        {
            Destroy(item);
        }
        buyItemRows.Clear();
        foreach (var item in sellItemRows)
        {
            Destroy(item);
        }
        sellItemRows.Clear();
    }

    private void DisplayItems(List<ItemSO> itemSOs, bool buyingFromTrader)
    {
        foreach (ItemSO itemSO in itemSOs)
        {
            Item item = new(itemSO);
            CreateItemRow(item, buyingFromTrader);
        }
    }

    private void CreateItemRow(Item item, bool buyingFromTrader)
    {
        Transform instantiateLocation = (buyingFromTrader) ? buyRowLocation : sellRowLocation;
        GameObject rowGameObject = Instantiate(shopItemRowPrefab, instantiateLocation);
        ShopItemRow shopItemRow = rowGameObject.GetComponent<ShopItemRow>();
        shopItemRow.forSale = buyingFromTrader;
        shopItemRow.Display(item);
        if (buyingFromTrader)
        {
            shopItemRow.buttonAction = BuyItem;
            shopItemRow.forSale = true;
            buyItemRows.Add(rowGameObject);
        }
        else
        {
            shopItemRow.buttonAction = SellItem;
            shopItemRow.forSale = false;
            sellItemRows.Add(rowGameObject);
        }
    }

    public void BuyItem(Item item)
    {
        if (playerInShop.money < item.itemValue) { return; }

        // add item to inventory
        // create copy of item to new reference
        Item itemCopy = new(item);
        playerInShop.items.Add(itemCopy);

        // update money total
        playerInShop.money -= item.itemValue;
        CalculateMoney();

        // update displayed items
        CreateItemRow(itemCopy, false);
    }

    public void SellItem(Item item)
    {
        // update displayed items
        GameObject itemRowToRemove = sellItemRows.First(row =>
        {
            ShopItemRow itemRow = row.GetComponent<ShopItemRow>();
            return itemRow.item.itemName.Equals(item.itemName);
        });

        if (itemRowToRemove == null)
        {
            Debug.LogError("SellItem() could not find item to sell");
            return;
        }

        // add money
        playerInShop.money += item.itemValue / 2;
        CalculateMoney();

        // remove item
        playerInShop.items.Remove(item);

        sellItemRows.Remove(itemRowToRemove);
        Destroy(itemRowToRemove);
    }
}