using UnityEngine;

using UnityEngine.UI;

public class DialogSystem : MonoBehaviour {

    public static DialogSystem Instance { get; set; }

    [SerializeField] private GameObject dialog, dialogPanel, dialogWithOptions;
    [SerializeField] private RectTransform optionsPanel, dialogWithOptionsScrollContent;
    
    void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DialogAwake();
	}

    private void DialogAwake()
    {
        dialogPanel.SetActive(false);
    }


    public void DisplayDialog(string npcName, string text, bool lastOne, DialogTree callbackDialogTree)
    {
        Text dialogueText = dialog.transform.Find("Text").GetComponent<Text>();
        Text nameTextUI = dialog.transform.Find("NPC Name").GetChild(0).GetComponent<Text>();
        DisplayCommonPart(npcName, text, nameTextUI, dialogueText);

        Button nextButton = dialog.transform.Find("Next").GetComponent<Button>();
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(delegate {
            if (!lastOne) callbackDialogTree.Continue();
            else callbackDialogTree.Finish();
        });
        nextButton.transform.GetChild(0).GetComponent<Text>().text = lastOne ? "Finish" : "Next";
    }

    public void DisplayDialogWithOptions(string npcName, string text, string[] options, DialogTree callbackDialogTree)
    {
        Text dialogueText = dialogWithOptionsScrollContent.Find("Text").GetComponent<Text>();
        Text nameTextUI = dialogWithOptions.transform.Find("NPC Name").GetChild(0).GetComponent<Text>();
        DisplayCommonPart(npcName, text, nameTextUI, dialogueText, withOptions: true);
        DestroyOptionsPanelChildren();
        InitOptions(options, callbackDialogTree);
    }

    public void Hide()
    {
        dialogPanel.gameObject.SetActive(false);
    }

    private void DestroyOptionsPanelChildren()
    {
        for (int i = 0; i < optionsPanel.childCount; i++)
        {
            Destroy(optionsPanel.GetChild(i).gameObject);
        }
    }

    private void InitOptions(string[] options, DialogTree callbackDialogTree)
    {
        foreach(string option in options)
        {
            GameObject dialogOptionObj = Instantiate(Resources.Load<GameObject>("UI/DialogOptionButtonPrefab"));
            Button optionButton = dialogOptionObj.GetComponent<Button>();
            optionButton.transform.GetChild(0).GetComponent<Text>().text = option;
            optionButton.onClick.AddListener(delegate { callbackDialogTree.OnOptionSelected(option); });

            dialogOptionObj.transform.SetParent(optionsPanel.transform, false);
        }
    }


    private void DisplayCommonPart(string npcName, string text, Text nameTextUI, Text textUI, bool withOptions = false)
    {
        textUI.text = text;
        nameTextUI.text = npcName;
        dialogPanel.SetActive(true);
        dialogWithOptions.SetActive(withOptions);
        dialog.SetActive(!withOptions);
    }


}
