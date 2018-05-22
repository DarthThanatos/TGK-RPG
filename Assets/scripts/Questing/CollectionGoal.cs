using UnityEngine;

public class CollectionGoal : Goal
{
    public string ItemName { get; set; }

    public CollectionGoal(Quest Quest, Phase Phase, string ItemName, string Description, bool Completed, int CurrentAmount, int RequiredAmount)
    {
        this.Quest = Quest;
        this.Phase = Phase;
        this.ItemName = ItemName;
        this.Description = Description;
        this.Completed = Completed;
        this.CurrentAmount = CurrentAmount;
        this.RequiredAmount = RequiredAmount;

        UIEventHandler.OnItemRemovedFromInventory += OnItemRemoved;

        Evaluate();

    }

    public override void Init()
    {
        base.Init();
        UIEventHandler.OnItemAddedToInventory += ItemPickedUp;
    }

    public override void UnInit()
    {
        UIEventHandler.OnItemAddedToInventory -= ItemPickedUp;
    }

    private void OnItemRemoved(Item item)
    {
        if (item.ItemName == ItemName && !Quest.Completed)
        {
            CurrentAmount = InventoryController.instance.CountItemsHavingName(ItemName);
            Evaluate();
            Phase.CheckGoals();
            QuestEventHandler.GoalUpdated(this);
        }
    }

    void ItemPickedUp(Item item)
    {
        if (!Phase.Active) return;
        if (item.ItemName == ItemName && !Quest.Completed)
        {
            CurrentAmount = InventoryController.instance.CountItemsHavingName(ItemName);
            Evaluate();
            Phase.CheckGoals();
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

    void OnDestroy()
    {
        UIEventHandler.OnItemRemovedFromInventory -= OnItemRemoved;
        UIEventHandler.OnItemAddedToInventory -= ItemPickedUp;
    }
}