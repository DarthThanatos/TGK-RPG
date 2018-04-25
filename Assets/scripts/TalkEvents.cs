using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkEvents : MonoBehaviour {

    public delegate void TalkHandler(string NPCName);
    public static event TalkHandler OnTalkedToNPC;

    public static void TalkedToNPC(string NPCName)
    {
        if(OnTalkedToNPC != null)
        {
            OnTalkedToNPC(NPCName);
        }
    }
}
