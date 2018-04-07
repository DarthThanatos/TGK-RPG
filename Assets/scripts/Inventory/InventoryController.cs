using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {
    public static InventoryController instance { get; set; }
    public Item sword;
    public Item staff;
    public Item potionLog;
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


        List<BaseStat> swordStats = new List<BaseStat>();
        swordStats.Add(new BaseStat(6, "Power", "Your power lvl. "));
        //sword = new Item(swordStats, "Sword_01");
        //staff = new Item(swordStats, "Staff_01");
        //potionLog = new Item(swordStats, "Potion", "Drink this to log sth cool", "Drink", "Log Potion", false);
        giveItem("Sword_01");
        giveItem("Staff_01");
        giveItem("Potion");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            playerWeaponController.EquipWeapon(sword);
            //consumableController.consumeItem(potionLog);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            playerWeaponController.EquipWeapon(staff);
        }

    }


    public void giveItem(string objectSlug)
    {
        Item item = ItemDatabase.instance.GetItem(objectSlug);
        playerItems.Add(ItemDatabase.instance.GetItem(objectSlug));
        Debug.Log(playerItems.Count + " items in inventory. Added: " + objectSlug);
        UIEventHandler.ItemAddedToInventory(item);
    }

    public void SetItemDetails(Item item, Button selectedButton)
    {
        inventoryDetailsPanel.SetItem(item, selectedButton);
    }

    public void EquipItem(Item itemToEquip)
    {
        playerWeaponController.EquipWeapon(itemToEquip);
    }

    public void ConsumeItem(Item itemToConsume)
    {
        consumableController.consumeItem(itemToConsume);
    }
}
