
using UnityEngine;

public class CollectionGoal : Goal
{
    public string ItemName { get; set; }

    public CollectionGoal(Quest Quest, string ItemName, string Description, bool Completed, int CurrentAmount, int RequiredAmount)
    {
        this.Quest = Quest;
        this.ItemName = ItemName;
        this.Description = Description;
        this.Completed = Completed;
        this.CurrentAmount = CurrentAmount;
        this.RequiredAmount = RequiredAmount;
        Evaluate();
    }

    public override void Init()
    {
        base.Init();
        UIEventHandler.OnItemAddedToInventory += ItemPickedUp;
    }

    void ItemPickedUp(Item item)
    {
        if (item.ItemName == ItemName)
        {
            CurrentAmount++;
            Evaluate();
        }
    }

    public override void Finish()
    {
        for (int i = 0; i < RequiredAmount; i++)
        {
            Debug.Log("Removing " + ItemName);
            InventoryController.instance.RemoveItemHavingName(ItemName);
        }
    }
}
