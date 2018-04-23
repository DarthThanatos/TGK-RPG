
using System;
using UnityEngine;
public class PickupItem : Interactable {

    public Item ItemDrop { get; set; }

    //private MyInventory inv;

    [SerializeField] private string ObjectSlug;

    void Start()
    {
        //inv = GameObject.Find("Inventory").GetComponent<MyInventory>();
        ItemDrop = ItemDatabase.instance.GetNewInstanceOfItemWithSlug(ObjectSlug);
    }

    public override void Interact(){
        Debug.Log("Interacting with pickup item  " + ItemDrop.ItemName );
        //inv.AddItem(ItemDrop.ObjectSlug);
        InventoryController.instance.giveItem(ItemDrop);
        Destroy(gameObject);
	}
}
