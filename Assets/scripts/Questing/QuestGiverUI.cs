
using UnityEngine;

public class QuestGiverUI : MonoBehaviour {

    private GameObject questSign;
    private bool shouldShowMessage = false;

    [SerializeField] private AudioSource updatedJournalSound, questCompletedSound;

    private string msgToShow;
    private Quest quest;

	void Start () {
	    questSign = Instantiate(Resources.Load<GameObject>("QuestSign"));
	}
	

    public void QuestStarted(Quest quest)
    {
        this.quest = quest;
        ShowMsg("Updated my Journal", 3f);
        updatedJournalSound.Play();
    }

    private void ShowMsg(string msgToShow, float period)
    {
        this.msgToShow = msgToShow;
        shouldShowMessage = true;
        Invoke("StopShowingMsg", period);
    }

    private void StopShowingMsg()
    {
        shouldShowMessage = false;
    }

    public void QuestCompleted()
    {
        questCompletedSound.Play();
        ShowMsg(
            "Quest Finished\n\n\nReward:\n\n" +
            "- Item: " + quest.ItemReward.ItemName + "\n" +
            "- Gold: " + quest.GoldReward + "\n" + 
            "- Experience: " + quest.ExperienceReward,
            6f
        );
        Destroy(questSign);
    }


    void OnGUI()
    {
        if (shouldShowMessage)
        {
            int textWidth = 200, textHeight = 30;
            Rect rect = new Rect(Screen.width/2 - textWidth / 2, Screen.height/2 - textHeight / 2, textWidth, textHeight * msgToShow.Split('\n').Length);
            GUI.Box(rect, msgToShow);
        }
    }
}
