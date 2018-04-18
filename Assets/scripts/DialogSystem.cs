using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DialogSystem : MonoBehaviour {

    public static DialogSystem instance { get; set; }

    public GameObject dialogPanel; 
    public string npcName;
    public List<string> dialogueLines = new List<string>();

    Button nextButton;
    Text dialogueText, nameText;
    int dialogueIndex;

    private Action actionAtFinish;

    void Awake() {
        nextButton = dialogPanel.transform.Find("Next").GetComponent<Button>();
        dialogueText = dialogPanel.transform.Find("Text").GetComponent<Text>();
        nameText = dialogPanel.transform.Find("NPC Name").transform.Find("Text").GetComponent<Text>();

        dialogPanel.SetActive(false);
        nextButton.onClick.AddListener(delegate { ContinueDialog(); });


        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
	}
	
    public void AddNewDialog(string[] lines, string npcName)
    {
        actionAtFinish = null;

        dialogueIndex = 0;
        dialogueLines = new List<string>(lines.Length);
        dialogueLines.AddRange(lines);
        Debug.Log("Added " + dialogueLines.Count + " to a dialog system");

        this.npcName = npcName;
        CreateDialog();
    }

    public void SetActionAtFinish(Action actionAtFinish)
    {
        this.actionAtFinish = actionAtFinish;
    }

    public void CreateDialog()
    {
        dialogueText.text = dialogueLines[dialogueIndex];
        nameText.text = npcName;
        dialogPanel.SetActive(true);
    }

    public void ContinueDialog()
    {
        if (dialogueIndex < dialogueLines.Count - 1)
        {
            dialogueIndex++;
            dialogueText.text = dialogueLines[dialogueIndex];
            if (dialogueIndex == dialogueLines.Count - 1)
                nextButton.transform.GetChild(0).GetComponent<Text>().text = "Finish";
        }
        else
        {
            dialogueIndex = 0;
            nextButton.transform.GetChild(0).GetComponent<Text>().text = "Next";
            dialogPanel.SetActive(false);
            if(actionAtFinish != null) actionAtFinish();

        }
        
    }

}
