using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal
{
    public string ItemID { get; set; }

    public CollectionGoal(Quest Quest, string ItemID, string Description, bool Completed, int CurrentAmount, int RequiredAmount)
    {
        this.Quest = Quest;
        this.ItemID = ItemID;
        this.Description = Description;
        this.Completed = Completed;
        this.CurrentAmount = CurrentAmount;
        this.RequiredAmount = RequiredAmount;
    }

    public override void Init()
    {
        base.Init();
        UIEventHandler.OnItemAddedToInventory += ItemPickedUp;
    }

    void ItemPickedUp(Item item)
    {
        if (item.ItemName == ItemID)
        {
            CurrentAmount++;
            Evaluate();
        }
    }

    public override void Finish()
    {
        for (int i = 0; i < RequiredAmount; i++)
        {
            Debug.Log("Removing " + ItemID);
            //InventoryController.instance.RemoveItemHavingName(ItemID);
        }
    }
}
