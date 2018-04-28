using System.Collections.Generic;

public class UltimateSlayer : Quest {
    
    public override void init(QuestGiver questGiver)
    {
        QuestName = "Ultimate Slime Slayer";
        Description = "An NPC told me to kill 5 slimes and to bring him 3 swords. After doing all this he asked me to go back to him so that he can reward me for my effort.";

        ItemReward = ItemDatabase.instance.GetNewInstanceOfItemWithSlug("Potion");
        ExperienceReward = 200;
        GoldReward = 500;
        IsMain = false;

        Phases = new List<Phase>();

        Phase phaseOne = new Phase(this, true, false);
        List<Goal> PhaseOneGoals = new List<Goal>();
        PhaseOneGoals.Add(new KillGoal(this, phaseOne, 0, "Kill 5 Slimes", false, 0, 5));
        PhaseOneGoals.Add(new CollectionGoal(this, phaseOne, "Sword", "Bring me 3 swords", false, InventoryController.instance.CountItemsHavingName("Sword"), 3));
        phaseOne.Goals = PhaseOneGoals;

        Phase phaseTwo = new Phase(this, true, false);
        List<Goal> PhaseTwoGoals = new List<Goal>();
        PhaseTwoGoals.Add(new TalkGoal(this, phaseTwo, "PasserBy", "Talk to", false, 0, 1));
        phaseTwo.Goals = PhaseTwoGoals;

        Phases.Add(phaseOne);
        Phases.Add(phaseTwo);

        phaseOne.Init();
    }

    public override bool StartingConditionsMet(QuestGiver questGiver)
    {
        return 
            new List<string>() { "after_first_acceptance_dialog", "after_second_acceptance_dialog" }
                .Contains(questGiver.dialogTree.GetState());
    }

    public override void OnQuestInProgress(QuestGiver questGiver)
    {
        base.OnQuestInProgress(questGiver);
        questGiver.dialogTree.MoveToState("state_2_dialog");
    }

    public override void Finish(QuestGiver questGiver)
    {
        base.Finish(questGiver);
        questGiver.dialogTree.MoveToState("state_3_dialog");
    }

    public override void OnAfterQuestFinished(QuestGiver questGiver)
    {
        base.OnAfterQuestFinished(questGiver);
        questGiver.dialogTree.MoveToState("state_4_dialog");

    }
}
