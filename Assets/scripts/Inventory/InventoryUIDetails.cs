﻿
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIDetails : MonoBehaviour {

    Item item;
    Button selectedItemButton, itemInteractedButton;
    Text itemNameText, itemDescriptionText, itemInteractButtonText, statText;


    void Start()
    {
        UIEventHandler.OnItemRemovedFromInventory += RemoveItemIfSelected;
        itemNameText = transform.Find("ItemName").GetComponent<Text>();
        statText = transform.Find("Stat_List").GetChild(0).GetComponent<Text>();
        itemDescriptionText = transform.Find("ItemDescription").GetComponent<Text>();
        itemInteractedButton = transform.Find("Button").GetComponent<Button>();
        itemInteractButtonText = itemInteractedButton.transform.GetChild(0).GetComponent<Text>();
        gameObject.SetActive(false);
    }

    public void SetItem(Item item, Button selectedButton)
    {
        this.item = item;

        selectedItemButton = selectedButton;
        itemNameText.text = item.ItemName;
        itemDescriptionText.text = item.Description;
        itemInteractButtonText.text = item.ActionName;
        statText.text = item.Stats != null ? item.Stats.Aggregate("", (acc, current) => acc + current.StatName + ": " + current.BaseValue + "\n") : "";
        itemInteractedButton.onClick.RemoveAllListeners();
        itemInteractedButton.onClick.AddListener(onItemInteract );

        gameObject.SetActive(true);
    }

    public void onItemInteract()
    {
        if (item.ItemType == Item.itemTypes.Consumable)
        {
           InventoryController.instance.ConsumeItem(item);
        }
        else if(item.ItemType == Item.itemTypes.Weapon)
        {
            InventoryController.instance.EquipItem(item);
        }
        UnselectItem();
    }

    private void RemoveItemIfSelected(Item item)
    {

        if (item == null || this.item == null) return;
        if (item.Uuid == this.item.Uuid)
        {
            UnselectItem();
        }
    }

    private void UnselectItem()
    {
        item = null;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        UIEventHandler.OnItemRemovedFromInventory -= RemoveItemIfSelected;
    }

}
