using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class JournalUIDetails : MonoBehaviour {

    [SerializeField] private RectTransform QuestDetailsPanel;
    [SerializeField] private Text QuestNameUI;
    [SerializeField] private Text QuestDescriptionUI;
    [SerializeField] private RectTransform CurrentGoalsStatePanel;

    private Dictionary<Goal, GameObject> goalToGameObjectMap;

    void Start () {
        goalToGameObjectMap = new Dictionary<Goal, GameObject>();
        QuestEventHandler.OnDisplayQuestDetails += OnDisplayQuestDetails;
        QuestEventHandler.OnGoalUpdated += OnGoalUpdated;
        QuestEventHandler.OnQuestFinished += OnQuestFinished;
        QuestEventHandler.OnUnselectQuest += OnUnselectQuest;
    }

    private void OnUnselectQuest()
    {
        QuestDetailsPanel.gameObject.SetActive(false);
    }

    private void OnQuestFinished(Quest quest)
    {
        QuestDetailsPanel.gameObject.SetActive(false);
    }

    private void OnGoalUpdated(Goal goal)
    {
        if (goal != null && goalToGameObjectMap.ContainsKey(goal))
        {
            goalToGameObjectMap[goal].transform.Find("Label").GetComponent<Text>().text = goal.GetGoalState();
            if (goal.Completed) goalToGameObjectMap[goal].GetComponent<Toggle>().isOn = true;
        }
    }

    private void OnDisplayQuestDetails(Quest quest)
    {
        QuestDetailsPanel.gameObject.SetActive(true);
        QuestNameUI.text = quest.QuestName;
        QuestDescriptionUI.text = quest.Description;
        DestroyOldGoals();
        InitNewGoals(quest);
    }

    private void DestroyOldGoals()
    {

        goalToGameObjectMap = new Dictionary<Goal, GameObject>();
        for (int i = 0; i < CurrentGoalsStatePanel.childCount; i++)
        {
            Destroy(CurrentGoalsStatePanel.GetChild(i).gameObject);
        }
    }
	
    private void InitNewGoals(Quest quest)
    {
        foreach(Goal goal in quest.Goals)
        {
            GameObject GoalState = Instantiate(Resources.Load<GameObject>("UI/GoalState"));
            GoalState.transform.Find("Label").GetComponent<Text>().text = goal.GetGoalState();
            GoalState.transform.SetParent(CurrentGoalsStatePanel, false);
            goalToGameObjectMap[goal] = GoalState;
            if (goal.Completed) goalToGameObjectMap[goal].GetComponent<Toggle>().isOn = true;
        }
    }

}
