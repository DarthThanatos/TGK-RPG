
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailsUI : MonoBehaviour {

    [SerializeField] private RectTransform QuestDetailsPanel;
    [SerializeField] private Text QuestNameUI;
    [SerializeField] private RectTransform GoalsContainer;

    private Dictionary<Goal, GameObject> goalToGameObjectMap;

    void Start () {
        QuestEventHandler.OnFollowQuest += OnFollowQuest;
        QuestEventHandler.OnUnFollowQuest += OnUnFollowQuest;
        QuestEventHandler.OnGoalUpdated += OnGoalUpdated;
        QuestEventHandler.OnQuestFinished += OnQuestFinished;
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
            goalToGameObjectMap[goal].GetComponent<Toggle>().isOn = goal.Completed;
        }
    }

    private void OnUnFollowQuest()
    {
        QuestDetailsPanel.gameObject.SetActive(false);
    }

    private void OnFollowQuest(Quest quest)
    {

        QuestDetailsPanel.gameObject.SetActive(true);
        QuestNameUI.text = quest.QuestName;
        DestroyOldGoals();
        InitNewGoals(quest);
    }


    private void DestroyOldGoals()
    {

        goalToGameObjectMap = new Dictionary<Goal, GameObject>();
        for (int i = 0; i < GoalsContainer.childCount; i++)
        {
            Destroy(GoalsContainer.GetChild(i).gameObject);
        }
    }

    private void InitNewGoals(Quest quest)
    {
        foreach (Goal goal in quest.Goals)
        {
            GameObject GoalState = Instantiate(Resources.Load<GameObject>("UI/GoalState"));
            GoalState.transform.Find("Label").GetComponent<Text>().text = goal.GetGoalState();
            GoalState.transform.SetParent(GoalsContainer, false);
            goalToGameObjectMap[goal] = GoalState;
            if (goal.Completed) goalToGameObjectMap[goal].GetComponent<Toggle>().isOn = true;
        }
    }

}
