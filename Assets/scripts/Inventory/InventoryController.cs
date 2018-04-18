
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    public static InventoryController instance { get; set; }

    public PlayerWeaponController playerWeaponController;
    public ConsumableController consumableController;
    public InventoryUIDetails inventoryDetailsPanel;

    public List<Item> playerItems = new List<Item>();

    void Start()
    {
        consumableController = GetComponent<ConsumableController>();
        playerWeaponController = GetComponent<PlayerWeaponController>();

        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else{
            instance = this;
        }

    }



    public void giveItem(string objectSlug)
    {
        Item item = ItemDatabase.instance.GetNewInstanceOfItemWithSlug(objectSlug);
        Debug.Log(playerItems.Count + " items in inventory. Added: " + objectSlug);
        UIEventHandler.ItemAddedToInventory(item);
    }


    public void giveItem(Item item)
    {
        Debug.Log(playerItems.Count + " items in inventory. Added: " + item.ObjectSlug);
        UIEventHandler.ItemAddedToInventory(item);

    }

    public int countItemsHavingName(string name)
    {
        return playerItems.FindAll(x => x.ItemName == name).Count;
    }

    public void SetItemDetails(Item item, Button selectedButton)
    {
        inventoryDetailsPanel.SetItem(item, selectedButton);
    }

    public void EquipItem(Item itemToEquip)
    {
        playerWeaponController.EquipWeapon(itemToEquip);
    }


    public void UnequipItem(Item itemToUnequip)
    {
        playerWeaponController.UnequipCurrentWeaponIfMatches(itemToUnequip);
    }

    public void ConsumeItem(Item itemToConsume)
    {
        consumableController.consumeItem(itemToConsume);
        UIEventHandler.ItemRemovedFromInventory(itemToConsume);
    }

    public void RemoveItem(Item item)
    {
        playerWeaponController.UnequipCurrentWeaponIfMatches(item);
        UIEventHandler.ItemRemovedFromInventory(item);
    }

    public void RemoveItemHavingName(string name)
    {
        Item itemByName = playerItems.Find(x => x.ItemName == name);
        RemoveItem(itemByName);
    }
}

