
public class NPC : Interactable {

    public string npcName;
    public string dialogFileName;

    public DialogTree dialogTree;

    public override void Interact(){

        dialogTree = dialogTree ?? new DialogTree(dialogFileName);
        dialogTree.StartDialog(npcName);

	}
}
