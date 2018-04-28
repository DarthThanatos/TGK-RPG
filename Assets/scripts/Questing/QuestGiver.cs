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
            base.Interact();
            dialogTree.SetActionAtFinish(delegate { TryToAssignQuest(); });
        }
        else if(QuestAssigned && !HelpedNPC)
        {
            CheckQuest();
        }
        else
        {
            dialogTree.StartDialog(npcName);
        }
    }

    void TryToAssignQuest()
    {
        Quest = (Quest)Quests.AddComponent(System.Type.GetType(QuestType));
        if (Quest.StartingConditionsMet(this))
        {
            QuestAssigned = true;
            Quest.init(this);
            Journal.instance.AddQuest(Quest);

            GetComponent<QuestGiverUI>().QuestStarted(Quest);
        }
        else
        {
            Destroy(Quests.GetComponent<Quest>());
        }

    }

    void CheckQuest()
    {
        Quest.CheckGoals();
        if (Quest.Completed)
        {
            Journal.instance.FinishQuestFrom(Quest, this);
            HelpedNPC = true;
            QuestAssigned = false;

            dialogTree.StartDialog(npcName);
            dialogTree.SetActionAtFinish(delegate {
                GetComponent<QuestGiverUI>().QuestCompleted();
                Quest.OnAfterQuestFinished(this);
            });
        }
        else
        {
            Quest.OnQuestInProgress(this);
            dialogTree.StartDialog(npcName);
        }
    }
}
