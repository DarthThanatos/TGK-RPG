using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSlayer : Quest {
    
	void Start () {
        QuestName = "Ultimate Slime Slayer";
        Description = "Crush Kill Demolish";
        ItemReward = ItemDatabase.instance.GetItem("Potion");
        ExpreienceReward = 200;

        Goals = new List<Goal>();
        Goals.Add(new KillGoal(this, 0, "Kill 5 Slimes", false, 5, 5));
        Goals.Add(new CollectionGoal(this, "Sword", "Bring me 3 swords", false, InventoryController.instance.countItemsHavingName("Sword"), 2));

        Goals.ForEach(x => x.Init());
	}
	
}
