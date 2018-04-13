
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour {

    [SerializeField] private RectTransform JournalPanel;
    [SerializeField] private RectTransform MainQuestsParentContent;
    [SerializeField] private RectTransform SideQuestsParentContent;
    [SerializeField] private Text Header;

    private bool isJournalActive = false;
    private bool showingToDoQuests = true;
    private Dictionary<Quest, GameObject> questToQuestSlot;

    private void ShowAppropriateQuests()
    {
        if (showingToDoQuests) ShowTodoQuests();
        else ShowDoneQuests();
    }

    private void ShowDoneQuests()
    {
        ShowQuests(Journal.instance.GetMainDoneQuests(), Journal.instance.GetSideDoneQuests());
    }

    private void ShowTodoQuests()
    {
        ShowQuests(Journal.instance.GetMainToDoQuests(), Journal.instance.GetSideTodoQuests());
    }

    private void ShowQuests(List<Quest> mainQuests, List<Quest> sideQuests)
    {
        AddQuestsToContainer(mainQuests, MainQuestsParentContent);
        AddQuestsToContainer(sideQuests, SideQuestsParentContent);
    }

    private void AddQuestsToContainer(List<Quest> quests, RectTransform container)
    {
        container.DetachChildren();
        foreach(Quest quest in quests)
        {
            questToQuestSlot[quest].transform.SetParent(container, false);
        }
    }

    public void OnShowToDoQuestsClicked()
    {
        if (!showingToDoQuests) QuestEventHandler.UnselectQuest();
        showingToDoQuests = true;
        Header.text = "Your journal: To-Do Quests";
        ShowTodoQuests();
    }

    public void OnShowDoneQuestsClicked()
    {
        if (showingToDoQuests) QuestEventHandler.UnselectQuest();
        showingToDoQuests = false;
        Header.text = "Your journal: Done Quests";
        ShowDoneQuests();
    }

    void Start () {
        questToQuestSlot = new Dictionary<Quest, GameObject>();
        QuestEventHandler.OnQuestAdded += OnQuestAdded;
        QuestEventHandler.OnQuestFinished += OnQuestFinished;
    }

    private void OnQuestFinished(Quest quest)
    {
        questToQuestSlot[quest].transform.Find("FollowQuestBtn")
            .GetComponent<Button>().transform.GetChild(0)
            .GetComponent<Text>().text = "Follow";
        ShowAppropriateQuests();
    }

    private void OnQuestAdded(Quest quest)
    {
        GameObject questSlot = Instantiate(Resources.Load<GameObject>("UI/Quest Slot"));
        Text titleText = questSlot.transform.Find("Title").GetComponent<Text>();
        Button followBtn = questSlot.transform.Find("FollowQuestBtn").GetComponent<Button>();

        titleText.text = quest.QuestName;
        followBtn.onClick.AddListener(delegate { FollowQuest(quest, followBtn); });
        questSlot.GetComponent<Button>().onClick.AddListener(delegate { DisplayQuestDetails(quest); });

        questToQuestSlot[quest] = questSlot;

        if (quest.IsMain)
        {
            Debug.Log("Adding main quest to journal ui");
            questSlot.transform.SetParent(MainQuestsParentContent, false);
        }
        else
        {
            Debug.Log("Adding side quest to journal ui");
            questSlot.transform.SetParent(SideQuestsParentContent, false);
        }

        ShowAppropriateQuests();
    }

    private void FollowQuest(Quest quest, Button followButton)
    {
        string text = followButton.transform.GetChild(0).GetComponent<Text>().text;
        if (text == "Follow")
        {
            QuestEventHandler.FollowQuest(quest);
        }
        else
        {
            QuestEventHandler.UnFollowQuest();

        }
        followButton.transform.GetChild(0).GetComponent<Text>().text = text == "Follow" ? "Unfollow" : "Follow"; 

    }

    private void DisplayQuestDetails(Quest quest)
    {
        QuestEventHandler.DisplayQuestDetails(quest);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.J))
        {
            isJournalActive = !isJournalActive;
            JournalPanel.gameObject.SetActive(isJournalActive);
            
        }
    }
}
