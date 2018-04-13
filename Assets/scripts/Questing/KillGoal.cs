using System;

class KillGoal : Goal
{
    public int EnemyID { get; set; }

    public KillGoal(Quest Quest, int EnemyID, string Description, Boolean Completed, int CurrentAmount, int RequiredAmount)
    {
        this.Quest = Quest;
        this.EnemyID = EnemyID;
        this.Description = Description;
        this.Completed = Completed;
        this.CurrentAmount = CurrentAmount;
        this.RequiredAmount = RequiredAmount;
    }

    public override void Init()
    {
        base.Init();
        CombatEvents.OnEnemyDeath += EnemyDied;
    }

    void EnemyDied(IEnemy enemy)
    {
        if(enemy.ID == EnemyID && !Completed)
        {
            CurrentAmount++;
            Evaluate();
            QuestEventHandler.GoalUpdated( this);
        }
    }

    public override string GetGoalState()
    {
        return "Killed " + EnemyCatalogue.enemies[EnemyID] + "s: " + CurrentAmount + "/" + RequiredAmount;
    }
}

