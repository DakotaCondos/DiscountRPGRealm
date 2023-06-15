using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : SceneSingleton<ShopManager>
{
    [Header("General")]
    public GameObject shopItemRowPrefab;
    public Transform buyRowLocation;
    public Transform sellRowLocation;
    public List<GameObject> buyItemRows = new();
    public List<GameObject> sellItemRows = new();

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
        BuildShop(TurnManager.Instance.GetCurrentActor().player.currentSpace.shopLevel);
    }

    private void OnDisable()
    {
        ResetShop();
    }


    private void BuildShop(int shopLevel)
    {
        // minimul level for a shop is 1.
        if (shopLevel < 1) { return; }

        for (int i = 0; i < shopLevel; i++)
        {
            DisplayItems(levelsItems[i], true);
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

    private void DisplayItems(List<ItemSO> itemSOs, bool sellingItem)
    {
        foreach (ItemSO itemSO in itemSOs)
        {
            Item item = new(itemSO);
            CreateItemRow(item, sellingItem);
        }
    }

    private void CreateItemRow(Item item, bool sellingItem)
    {
        Transform instantiateLocation = (sellingItem) ? sellRowLocation : buyRowLocation;
        GameObject g = Instantiate(shopItemRowPrefab, instantiateLocation);
        ShopItemRow j = g.GetComponent<ShopItemRow>();
        j.forSale = sellingItem;
        j.Display(item);
        if (sellingItem)
        {
            sellItemRows.Add(g);
        }
        else
        {
            buyItemRows.Add(g);
        }
    }

    public void buyItem(Item item)
    {

    }

    public void sellItem(Item item)
    {

    }
}