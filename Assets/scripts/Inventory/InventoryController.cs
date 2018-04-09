
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

        UIEventHandler.OnItemUnequipped += ItemUnequipped;

        giveItem("Sword_01");
        giveItem("Staff_01");
        giveItem("Potion");
    }



    public void giveItem(string objectSlug)
    {
        Item item = ItemDatabase.instance.GetItem(objectSlug);
        playerItems.Add(ItemDatabase.instance.GetItem(objectSlug));
        Debug.Log(playerItems.Count + " items in inventory. Added: " + objectSlug);
        UIEventHandler.ItemAddedToInventory(item);
        
    }


    public void giveItem(Item item)
    {
        playerItems.Add(item);
        Debug.Log(playerItems.Count + " items in inventory. Added: " + item.ObjectSlug);
        UIEventHandler.ItemAddedToInventory(item);

    }

    public void SetItemDetails(Item item, Button selectedButton)
    {
        inventoryDetailsPanel.SetItem(item, selectedButton);
    }

    public void EquipItem(Item itemToEquip)
    {
        Item previousWeaponItem = playerWeaponController.EquipWeapon(itemToEquip);
        if(previousWeaponItem != null)
        {
            UIEventHandler.ItemAddedToInventory(previousWeaponItem);
        }
    }

    void ItemUnequipped(Item item)
    {
       if(item != null) {
            UIEventHandler.ItemAddedToInventory(item);
            playerWeaponController.UnequipCurrentWeapon(notify : true);
        }

    }

    public void ConsumeItem(Item itemToConsume)
    {
        consumableController.consumeItem(itemToConsume);
        playerItems.Remove(itemToConsume);
        UIEventHandler.ItemRemovedFromInventory(itemToConsume);
    }
}
