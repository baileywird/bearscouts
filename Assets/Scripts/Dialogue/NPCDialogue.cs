using UnityEngine;

[System.Serializable]

public class DialogueChoice
{
	public int dialogueIndex;
    public string[] choices;
    public int[] nextDialogueIndexes; //where choice leads
    public bool[] givesQuest; //if choice gives quesst
}

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]

public class NPCDialogue : ScriptableObject
{
	public string npcName;
	public Sprite npcPortrait;
	public string[] dialogueLines;
	public bool[] autoProgressLines;
	public bool[] endDialogueLines; //mark where dialogue ends
	public float autoProgressDelay = 1.5f;
	public float typingSpeed = 0.05f;
	public AudioClip voiceSound;
	public float voicePitch = 1f;

	public DialogueChoice[] choices;

	public int questInProgressIndex; //dialogue when quest is in progress
	public int questCompletedIndex; //dialogue when quest is completed
	public Quest quest; //the quest npc gives

}
