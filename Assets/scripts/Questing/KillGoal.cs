using System;
using UnityEngine;

class KillGoal : Goal
{
    public int EnemyID { get; set; }
    private Vector3 goalArea = new Vector3(-40, 2.5f, -54);

    private GameObject questPointer; //for 3D navigation
    private GameObject navigationUI; //for 2D navigation


    public KillGoal(Quest Quest, Phase Phase, int EnemyID, string Description, Boolean Completed, int CurrentAmount, int RequiredAmount)
    {
        this.Quest = Quest;
        this.Phase = Phase;
        this.EnemyID = EnemyID;
        this.Description = Description;
        this.Completed = Completed;
        this.CurrentAmount = CurrentAmount;
        this.RequiredAmount = RequiredAmount;
    }

    public override void Init()
    {
        base.Init();
        InitNavigationPanel();

        CombatEvents.OnEnemyDeath += EnemyDied;
    }

    void EnemyDied(IEnemy enemy)
    {
        if (!Phase.Active) return;
        if (enemy.ID == EnemyID && !Completed)
        {
            CurrentAmount++;
            Evaluate();
            Phase.CheckGoals();
            Navigation2D();
            QuestEventHandler.GoalUpdated(this);
        }
    }

    public override void UnInit()
    {
        UnityEngine.Object.Destroy(navigationUI);
        CombatEvents.OnEnemyDeath -= EnemyDied;
    }


    public override string GetGoalState()
    {
        return "Killed " + EnemyCatalogue.enemies[EnemyID] + "s: " + CurrentAmount + "/" + RequiredAmount;
    }


    private void Navigation2D()
    {
        if (Completed || !Phase.Active)
        {
            UnityEngine.Object.Destroy(navigationUI);
        }
        else
        {
            if (navigationUI == null)
            {
                InitNavigationPanel();
            }
        }

    }

    private void InitNavigationPanel()
    {
        navigationUI = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Navigation/NavigationPanel"));
        navigationUI.GetComponent<NavigationUI>().Init(goalArea);
        navigationUI.transform.SetParent(GameObject.Find("Canvas").gameObject.transform);
    }

    private void Navigation3D()
    {
        if (Completed || !Phase.Active)
        {
            UnityEngine.Object.Destroy(questPointer);
        }
        else
        {
            if (questPointer != null)
            {
                InitQuestPointer();
            }
        }
    }

    private void InitQuestPointer()
    {
        questPointer = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("QuestArrow"));
        questPointer.GetComponent<QuestArrow>().Init(goalArea);
    }
}

