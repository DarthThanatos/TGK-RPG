using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable {

    public string[] dialog;
    public string npcName;

	public override void Interact(){
        DialogSystem.instance.AddNewDialog(dialog, npcName);
		Debug.Log ("Interacting with an NPC");
	}
}
