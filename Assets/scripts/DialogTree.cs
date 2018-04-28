using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System;

public class DialogTree {

    private JObject dialogTreeJObject;

    private static string STATE = "state";
    private static string TEXT = "text";
    private static string OPTIONS = "options";
    private static string START_STATE = "start_state";
    private static string INDEX = "index";

    private string currentState;
    private int currentIndexInState;

    private IList<string> keys;
    private string npcName;

    private Action actionAtFinish;

    public DialogTree(string dialogFilePath)
    {
        dialogTreeJObject = JObject.Parse(File.ReadAllText("Assets/Resources/DialogDB/" + dialogFilePath));

        currentState = dialogTreeJObject[START_STATE].ToString();
        currentIndexInState = 0;

        keys = GetKeys(dialogTreeJObject);
    }

    private IList<string> GetKeys(JObject jObject)
    {
        return jObject.Properties().Select(p => p.Name).ToList();
    }
    
    public void StartDialog(string npcName)
    {
        actionAtFinish = null;
        this.npcName = npcName;
        ProcessCurrentDialogNode();
    }

    private bool CurrentTextHasOptions()
    {
        JObject currentText = GetCurrentTextNode();
        return GetKeys(currentText).Contains(OPTIONS);
    }

    private JObject GetCurrentTextNode()
    {
        JArray currentDialog = dialogTreeJObject[currentState].ToObject<JArray>();
        return currentDialog[currentIndexInState].ToObject<JObject>();
    }

    private void ProcessCurrentDialogNode()
    {
        bool currHasOptions = CurrentTextHasOptions();
        if (currHasOptions)
        {
            ProcessTextWithOptions();
        }
        else
        {
            ProcessUsualText();
        }

    }

    private void ProcessTextWithOptions()
    {
        JObject currTextNode = GetCurrentTextNode();
        string textContent = currTextNode[TEXT].ToObject<string>();
        string[] options = currTextNode[OPTIONS].ToObject<JArray>().ToList().Select(x => x[TEXT].ToObject<string>()).ToArray();
        DialogSystem.Instance.DisplayDialogWithOptions(npcName, textContent, options, this);
    }


    private void ProcessUsualText()
    {
        string textContent = GetCurrentTextNode()[TEXT].ToObject<string>();
        DialogSystem.Instance.DisplayDialog(npcName, textContent,  CurrentTextLastOne(), this);
    }

    private bool CurrentTextLastOne()
    {
        int index = GetCurrentTextNode()[INDEX].ToObject<int>();
        return index == dialogTreeJObject[currentState].ToObject<JArray>().Count();
    }

    public void Continue()
    {
        currentIndexInState++;
        ProcessCurrentDialogNode();
    }

    public void SetActionAtFinish(Action actionAtFinish)
    {
        this.actionAtFinish = actionAtFinish;
    }

    public void Finish()
    {
        JObject currTextNode = GetCurrentTextNode();
        if (GetKeys(currTextNode).Contains(STATE))
        {
            currentState = currTextNode[STATE].ToObject<string>();
        }

        currentIndexInState = 0;

        DialogSystem.Instance.Hide();
        if ( actionAtFinish != null ) actionAtFinish();
    }

    public void OnOptionSelected(string option)
    {
        JObject optionObj = 
            GetCurrentTextNode()[OPTIONS].ToObject<JArray>().ToList()
            .Where(x => x[TEXT].ToObject<string>() == option).First().ToObject<JObject>();

        if (GetKeys(optionObj).Contains(STATE))
        {
            currentState = optionObj[STATE].ToObject<string>();
        }

        currentIndexInState = 0;

        ProcessCurrentDialogNode();
    }

    public void MoveToState(string state)
    {
        if (keys.Contains(state))
        {
            currentState = state;
        }
    }

    public string GetState()
    {
        return currentState;
    }

}
