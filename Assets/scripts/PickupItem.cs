
using System;
using UnityEngine;
public class PickupItem : Interactable {

    public Item ItemDrop { get; set; }

    [SerializeField] private string ObjectSlug;

    void Start()
    {
        ItemDrop = ItemDatabase.instance.GetItem(ObjectSlug);
    }

    public override void Interact(){
        Debug.Log("Interacting with pickup item");
        InventoryController.instance.giveItem(ItemDrop);
        Destroy(gameObject);
	}
}
