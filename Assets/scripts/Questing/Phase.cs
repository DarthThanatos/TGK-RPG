
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Phase {

    public Quest Quest { get; set; }
    public List<Goal> Goals { get; set; }
    public bool Active = false;
    public bool Completed { get; set; }

    public Phase(Quest Quest, bool Active, bool Completed)
    {
        this.Quest = Quest;
        Goals = new List<Goal>();
        this.Active = Active;
        this.Completed = Completed;
    }

    public void Finish()
    {
        Goals.ForEach(x => x.Finish());
    }
    
    public void CheckGoals()
    {
        Completed = Goals.All(x => x.Completed);
        Quest.RefreshPhasesStates();
        Debug.Log("Phase completed");

    }

    public void UnInit()
    {
        Goals.ForEach(x => x.UnInit());
    }

    internal void Init()
    {
        Goals.Where(x => !x.Completed).ToList().ForEach(x => x.Init());
    }
}
