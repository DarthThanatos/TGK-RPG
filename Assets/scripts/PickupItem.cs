
using System;
using UnityEngine;
public class PickupItem : Interactable {

    public Item ItemDrop { get; set; }

    private MyInventory inv;

    [SerializeField] private string ObjectSlug;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<MyInventory>();
        ItemDrop = ItemDatabase.instance.GetNewInstanceOfItemWithSlug(ObjectSlug);
    }

    public override void Interact(){
        Debug.Log("Interacting with pickup item");
        inv.AddItem(ItemDrop.ObjectSlug);
        Destroy(gameObject);
	}
}
