using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEventHandler : MonoBehaviour {

    public delegate void QuestHandler(Quest quest);
    public static event QuestHandler OnQuestAdded;
    public static event QuestHandler OnQuestFinished;
    public static event QuestHandler OnDisplayQuestDetails;
    public static event QuestHandler OnQuestStateChanged;


    public delegate void QuestUnselecter();
    public static event QuestUnselecter OnUnFollowQuest;
    public static event QuestHandler OnFollowQuest;
    public static event QuestUnselecter OnUnselectQuest;

    public delegate void GoalHandler(Goal goal);
    public static event GoalHandler OnGoalUpdated;

    public static void QuestStateChanged(Quest quest)
    {
        if(OnQuestStateChanged != null)
        {
            OnQuestStateChanged(quest);
        }
    }

    public static void AddedQuest(Quest quest)
    {
        if(OnQuestAdded != null)
        {
            OnQuestAdded(quest);
        }
    }

    public static void FinishedQuest(Quest quest)
    {
        if(OnQuestFinished != null)
        {
            OnQuestFinished(quest);
        }
    }

    public static void GoalUpdated(Goal goal)
    {
        if(OnGoalUpdated != null)
        {
            OnGoalUpdated( goal);
        }
    }

    public static void DisplayQuestDetails(Quest quest)
    {
        if (OnDisplayQuestDetails != null)
        {
            OnDisplayQuestDetails(quest);
        }
    }

    public static void FollowQuest(Quest quest)
    {
        if (OnFollowQuest != null)
        {
            OnFollowQuest(quest);
        }
    }


    public static void UnFollowQuest()
    {
        if (OnUnFollowQuest != null)
        {
            OnUnFollowQuest();
        }
    }

    public static void UnselectQuest()
    {
        if (OnUnselectQuest != null)
        {
            OnUnselectQuest();
        }
    }
}
