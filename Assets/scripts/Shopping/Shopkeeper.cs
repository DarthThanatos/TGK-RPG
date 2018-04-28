using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Shopkeeper : Interactable {

    [SerializeField] private GameObject economySystemGameObj;
    [SerializeField] private string shopDBFileName;

    private ShopKeeperUI shopKeeperUI;

    private List<Item> shopItems;
    private int currentBalance = 0;
    public float priceMultiplier = .5f;

    void Start()
    {
        shopKeeperUI = economySystemGameObj.GetComponent<ShopKeeperUI>();
        Dictionary<string, int>  initialShopItemsAmounts =  newInitialShopItemsAmountsDict();
        initShopItems(initialShopItemsAmounts);
    }

    private Dictionary<string, int> newInitialShopItemsAmountsDict()
    {
        Dictionary<string, int> initialShopItemsAmounts  = new Dictionary<string, int>();

        StreamReader reader = new StreamReader("Assets/Resources/ShopDB/" + shopDBFileName);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] dbRecord = line.Split(' ');
            initialShopItemsAmounts[dbRecord[0]] = Int32.Parse(dbRecord[1]);
        }
        reader.Close();
        return initialShopItemsAmounts;
    }

    private void initShopItems(Dictionary<string, int> initialShopItemsAmounts)
    {
        shopItems = new List<Item>();
        foreach (string objectSlug in initialShopItemsAmounts.Keys)
        {
            for(int i = 0; i < initialShopItemsAmounts[objectSlug]; i++)
            {
                shopItems.Add(ItemDatabase.instance.GetNewInstanceOfItemWithSlug(objectSlug));
            }
        }
    }

    public void deleteObjectHavingSlug(string objectSlug)
    {
        Item itemToRemove = shopItems.Find(x => x.ObjectSlug == objectSlug);
        shopItems.Remove(itemToRemove);
    }

    public override void Interact()
    {
        shopKeeperUI.OpenShopDialog(this, shopItems, priceMultiplier);
    }

    public void ChangeBalance(int cost, bool selected, bool playerItem)
    {
        if (selected)
        {
            if (playerItem)
            {
                currentBalance += cost;
            }
            else
            {
                currentBalance -= cost;
            }
        }
        else
        {
            if (playerItem)
            {
                currentBalance -= cost;
            }
            else
            {
                currentBalance += cost;
            }
        }
        EconomyEventHandel.BalanceChanged(currentBalance);
    }

    public void CancelTransaction()
    {
        currentBalance = 0;
    }

    public void MakeTransaction(List<Item> boughtItems, List<Item> soldItems)
    {
        if (currentBalance < 0 && EconomySystem.instance.PlayerMoney < Math.Abs(currentBalance))
        {
            shopKeeperUI.DisplayMessage("You do not have enough gold to finalize this transaction");
            return;
        }

        EconomySystem.instance.PlayerMoney += currentBalance;
        currentBalance = 0;

        foreach(Item item in boughtItems)
        {
            shopItems.Remove(item);
            InventoryController.instance.giveItem(item);
        }
        foreach(Item item in soldItems)
        {
            shopItems.Add(item);
            InventoryController.instance.RemoveItem(item);
        }

        shopKeeperUI.RefreshShopDialog(shopItems, priceMultiplier);
    }


}
