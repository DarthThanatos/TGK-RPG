
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
        UIEventHandler.OnItemRemovedFromInventory += OnItemRemoved;
    }

    private void OnItemRemoved(Item item)
    {
        if (item.ItemName == ItemName && !Quest.Completed)
        {
            CurrentAmount--;
            Evaluate();
            QuestEventHandler.GoalUpdated(this);
        }
    }

    void ItemPickedUp(Item item)
    {
        if (item.ItemName == ItemName && !Quest.Completed)
        {
            CurrentAmount++;
            Evaluate();
            QuestEventHandler.GoalUpdated(this);
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

    public override string GetGoalState()
    {
        return "Gathered " + ItemName + "s: " + CurrentAmount + "/" + RequiredAmount;
    }
}
