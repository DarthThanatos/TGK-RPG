using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSlayer : Quest {

    public override void init()
    {
        QuestName = "Ultimate Slime Slayer";
        Description = "An NPC told me to kill 5 slimes and to bring him 3 swords. After doing all this he asked me to go back to him so that he can reward me for my effort.";

        ItemReward = ItemDatabase.instance.GetNewInstanceOfItemWithSlug("Potion");
        ExperienceReward = 200;
        GoldReward = 500;
        IsMain = false;

        Goals = new List<Goal>();
        Goals.Add(new KillGoal(this, 0, "Kill 5 Slimes", false, 4, 5));
        Goals.Add(new CollectionGoal(this, "Sword", "Bring me 3 swords", false, InventoryController.instance.CountItemsHavingName("Sword"), 3));

        Goals.ForEach(x => x.Init());
    }

	
}
