using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour {

    public static Journal instance { get; set; }
    private List<Quest> QuestList;

	void Start () {
        QuestList = new List<Quest>();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void AddQuest(Quest quest)
    {
        QuestList.Add(quest);
        QuestEventHandler.AddedQuest(quest);
    }

    public void FinishQuestFrom(Quest quest, QuestGiver questGiver)
    {
        quest.GiveReward();
        quest.Finish(questGiver);
        QuestEventHandler.FinishedQuest(quest);
    }
	
    public List<Quest> GetMainToDoQuests()
    {
        return QuestList.FindAll(x => x.IsMain && !x.Completed);
    }

    public List<Quest> GetMainDoneQuests()
    {
        return QuestList.FindAll(x => x.IsMain && x.Completed);
    }

    public List<Quest> GetSideDoneQuests()
    {
        return QuestList.FindAll(x => !x.IsMain && x.Completed);
    }

    public List<Quest> GetSideTodoQuests()
    {
        return QuestList.FindAll(x => !x.IsMain && !x.Completed);
    }


}
