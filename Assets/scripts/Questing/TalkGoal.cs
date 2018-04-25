using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkGoal : Goal {

    private string NPCName;
    private Vector3 goalArea;
    private GameObject navigationUI; //for 2D navigation

    public TalkGoal(Quest Quest, Phase Phase, string NPCName, string Description, Boolean Completed, int CurrentAmount, int RequiredAmount)
    {
        this.Quest = Quest;
        this.Phase = Phase;
        this.NPCName = NPCName;
        this.Description = Description;
        this.Completed = Completed;
        this.CurrentAmount = CurrentAmount;
        this.RequiredAmount = RequiredAmount;

        goalArea = GameObject.Find(NPCName).transform.position;
    }


    public override void Init()
    {
        base.Init();
        InitNavigationPanel();
        TalkEvents.OnTalkedToNPC += OnTalkedToNPC;
    }

    private void OnTalkedToNPC(string NPCName)
    {

        if (!Phase.Active) return;
        if (this.NPCName == NPCName && !Completed)
        {
            CurrentAmount++;
            Evaluate();
            Phase.CheckGoals();
            QuestEventHandler.GoalUpdated(this);
        }
    }

    public override string GetGoalState()
    {
        return "Talked to " + NPCName + ": " + CurrentAmount + "/" + RequiredAmount;
    }

    public override void Evaluate()
    {
        base.Evaluate();
        Navigation2D();
        Debug.Log("2d");
    }

    public override void UnInit()
    {
        UnityEngine.Object.Destroy(navigationUI);
        TalkEvents.OnTalkedToNPC -= OnTalkedToNPC;
    }

    private void Navigation2D()
    {
        if (Completed || !Phase.Active)
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
}
