using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class Quest : MonoBehaviour {
    public List<Goal> Goals { get; set; }
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public Item ItemReward { get; set; }
    public bool Completed { get; set; }
    public bool IsMain { get; set; }
    public int GoldReward { get; set; }
    

    public void CheckGoals()
    {
        Debug.Log("Quest completed");
        Completed = Goals.All(x => x.Completed);
    }

    public abstract void init();

    public void GiveReward()
    {
        if(ItemReward != null)
        {
            InventoryController.instance.giveItem(ItemReward);
        }
        EconomySystem.instance.PlayerMoney += GoldReward;
    }

    public void Finish()
    {
        Goals.ForEach(x => x.Finish());
    }
}
