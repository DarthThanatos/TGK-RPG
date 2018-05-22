
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    public static InventoryController instance { get; set; }

    private PlayerWeaponController playerWeaponController;
    private ConsumableController consumableController;
    public InventoryUIDetails inventoryDetailsPanel;

    public List<Item> playerItems = new List<Item>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        playerItems = new List<Item>();
    }


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

        for(int i = 0; i < 16; i++)
        {
            playerItems.Add(null);
            playerItems[i] = new Item();
        }

        giveItem("Sword_01");
        giveItem("Sword_01" );
        giveItem("Staff_01");
        giveItem("Potion");
        giveItem("Potion");
        giveItem("Potion");
        giveItem("Potion");
        giveItem("Potion");
        giveItem("Potion");
        giveItem("Potion");
        giveItem("Potion");
        giveItem("Potion");

    }



    public void giveItem(string objectSlug)
    {
        Item item = ItemDatabase.instance.GetNewInstanceOfItemWithSlug(objectSlug);
        AddItemToInventory(item);
    }



    public void giveItem(Item item)
    {
        AddItemToInventory(item);
    }


    private void AddItemToInventory(Item item)
    {
        playerItems.Add(item);
        UIEventHandler.ItemAddedToInventory(item);
    }

    public int CountItemsHavingName(string name)
    {
        return playerItems.FindAll(x => x.ItemName == name).Count; // + playerWeaponController.NumberOfItemsWithName(name);
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
        playerItems.Remove(itemToConsume);
        UIEventHandler.ItemRemovedFromInventory(itemToConsume);
    }

    public void RemoveItem(Item item)
    {
        playerWeaponController.UnequipCurrentWeaponIfMatches(item);
        playerItems.Remove(item);
        UIEventHandler.ItemRemovedFromInventory(item);
    }

    public void RemoveItemHavingName(string name)
    {
        Item itemByName = playerItems.Find(x => x.ItemName == name);
        RemoveItem(itemByName);
    }


    public Item FindOneOfName(string name)
    {
        return playerItems.Find(x => x.ItemName == name);
    }

    public List<Item> NotEquippedItemsList()
    {
        return playerItems.FindAll(x => !playerWeaponController.HasItemEquiped(x));
    }
    
    public string UUIDToName(System.Guid uuid)
    {
        return playerItems.Find(x => x.Uuid == uuid).ItemName;
    }
}

