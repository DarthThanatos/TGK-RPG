using System;
using UnityEngine;

class KillGoal : Goal
{
    public int EnemyID { get; set; }
    private Vector3 goalArea = new Vector3(-40, 2.5f, -65);

    private GameObject questPointer; //for 3D navigation
    private GameObject navigationUI; //for 2D navigation


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
        InitNavigationPanel();

        CombatEvents.OnEnemyDeath += EnemyDied;
    }

    void EnemyDied(IEnemy enemy)
    {
        if (enemy.ID == EnemyID && !Completed)
        {
            CurrentAmount++;
            Evaluate();
            QuestEventHandler.GoalUpdated(this);
        }
    }

    public override string GetGoalState()
    {
        return "Killed " + EnemyCatalogue.enemies[EnemyID] + "s: " + CurrentAmount + "/" + RequiredAmount;
    }

    public override void Evaluate()
    {
        base.Evaluate();
        Navigation2D();
        Debug.Log("2d");
    }

    private void Navigation2D()
    {
        if (Completed)
        {
            UnityEngine.Object.Destroy(navigationUI);
        }
        else
        {
            if (navigationUI != null)
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
        if (Completed)
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

