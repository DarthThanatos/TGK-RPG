using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class Quest : MonoBehaviour
{
    public List<Phase> Phases { get; set; }
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceReward { get; set; }
    public Item ItemReward { get; set; }
    public bool Completed { get; set; }
    public bool IsMain { get; set; }
    public int GoldReward { get; set; }

    private int currentPhase = 0;

    public void CheckGoals()
    {
        Phases.ForEach(x => x.CheckGoals());
        Completed = Phases.All(x => x.Completed);
    }

    public abstract void init();

    public void GiveReward()
    {
        if (ItemReward != null)
        {
            InventoryController.instance.giveItem(ItemReward);
        }
        EconomySystem.instance.PlayerMoney += GoldReward;
    }

    public void Finish()
    {
        Phases.ForEach(x => x.Finish());
    }

    public List<Goal> GoalsUpToCurrentPhase(){
        List<Goal> res = new List<Goal>();
        for (int i = 0; i < currentPhase +1; i++)
        {
            res.AddRange(Phases[i].Goals);
        }
        //return Phases.Where(x => x.Completed).SelectMany(x => x.Goals).ToList();
        return res;
    }

    public void RefreshPhasesStates()
    {
        Phases.ForEach(x => x.Active = false);
        Phases.ForEach(x => x.UnInit());

        Phase activePhase = null;
        for (int i = 0; i < Phases.Count; i++)
        {
            if( !Phases[i].Completed && activePhase == null)
            {
                activePhase = Phases[i];
                activePhase.Active = true;
                currentPhase = i;
            }
        }

        Phases[currentPhase].Init();
        QuestEventHandler.QuestStateChanged(this);
    }
}
