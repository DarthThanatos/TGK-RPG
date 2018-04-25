
using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour {

    public int Level { get; set; }

    public int CurrentExperience { get; set; }

    public int RequiredExperience { get { return Level * 25; }  }

    [SerializeField] private AudioSource newLvlSound;

    private bool shouldShowMessage = false;


    // Use this for initialization
    void Start () {
        CombatEvents.OnEnemyDeath += EnemyToExperience;
        QuestEventHandler.OnQuestFinished += OnQuestFinished;
        Level = 1;	
	}

    private void OnQuestFinished(Quest quest)
    {
        GrantExperience(quest.ExperienceReward);
    }

    public void EnemyToExperience(IEnemy enemy)
    {
        GrantExperience(enemy.Experience);
    }

    public void GrantExperience(int amount)
    {
        CurrentExperience += amount;
        while (CurrentExperience >= RequiredExperience)
        {
            CurrentExperience -= RequiredExperience;
            Level++;
            ShowNewLvlMsg();
        }
        UIEventHandler.PlayerLevelChanged();
    }


    private void ShowNewLvlMsg()
    {
        shouldShowMessage = true;
        newLvlSound.Play();
        Invoke("StopShowingMsg", 6f);
    }

    private void StopShowingMsg()
    {
        shouldShowMessage = false;
    }

    void OnGUI()
    {
        if (shouldShowMessage)
        {
            int textWidth = 200, textHeight = 30;
            Rect rect = new Rect(Screen.width / 2 - textWidth / 2, Screen.height / 2 - textHeight / 2, textWidth, textHeight);
            GUI.Box(rect, "New Level: " + Level);
        }
    }
}
