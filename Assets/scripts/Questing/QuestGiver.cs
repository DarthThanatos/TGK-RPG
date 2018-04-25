
using UnityEngine;

public class QuestGiver : NPC {

    public bool QuestAssigned { get; set; }
    public bool HelpedNPC { get; set; }

    [SerializeField] private GameObject Quests;
    [SerializeField] private string QuestType;

    private Quest Quest { get; set; }
    private QuestGiverUI QuestGiverUI;


    public override void Interact()
    {
        TalkEvents.TalkedToNPC(npcName);
        if (!QuestAssigned && !HelpedNPC)
        {
            Debug.Log("First");
            base.Interact();
            AssignQuest();
        }
        else if(QuestAssigned && !HelpedNPC)
        {
            Debug.Log("Second");
            CheckQuest();
        }
        else
        {
            Debug.Log("Third");
            DialogSystem.instance.AddNewDialog(new string[] { "Thanks for the last one.", "You are always welcome here." }, name);
        }
    }

    void AssignQuest()
    {
        QuestAssigned = true;
        Quest = (Quest) Quests.AddComponent(System.Type.GetType(QuestType));
        Quest.init();
        Journal.instance.AddQuest(Quest);
        DialogSystem.instance.SetActionAtFinish(delegate { GetComponent<QuestGiverUI>().QuestStarted(Quest); });
        
    }

    void CheckQuest()
    {
        Quest.CheckGoals();
        if (Quest.Completed)
        {
            Debug.Log("Giving reward");
            Journal.instance.FinishQuest(Quest);
            HelpedNPC = true;
            QuestAssigned = false;
            DialogSystem.instance.AddNewDialog(new string[] { "Thanks for that!", "Here is your reward!"}, name);
            DialogSystem.instance.SetActionAtFinish( delegate { GetComponent<QuestGiverUI>().QuestCompleted(); });
        }
        else
        {
            DialogSystem.instance.AddNewDialog(new string[] { "You have not done the quest.", "Please go and do it." }, name);
        }
    }
}
