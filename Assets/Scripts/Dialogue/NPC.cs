using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
	public NPCDialogue dialogueData;
	public GameObject dialoguePanel;
	public TMP_Text dialogueText, nameText;
	public Image portraitImage;
	public string npcID;

	private DialogueController dialogueUI;
	private int dialogueIndex;
	private bool isTyping, isDialogueActive;

	private enum QuestState { NotStarted, InProgress, Completed }
	private QuestState questState = QuestState.NotStarted;

	private void Start()
	{
		dialogueUI = DialogueController.Instance;
	}

	public bool CanInteract()
	{
		return !isDialogueActive;
	}

	public void Interact()
	{
		if (isDialogueActive)
		{
			NextLine();
		}
		else
		{
			StartDialogue();
		}
	}

	void StartDialogue()
	{
		//sync w quest data
		SyncQuestState();

		//set dialogue line based on quest state
		if(questState == QuestState.NotStarted)
		{
			dialogueIndex = 0;
		}
		else if (questState == QuestState.InProgress)
		{
			dialogueIndex = dialogueData.questInProgressIndex;
		}
		else if (questState == QuestState.Completed)
		{
			dialogueIndex=dialogueData.questCompletedIndex;
		}

			isDialogueActive = true;

		dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
		dialogueUI.ShowDialogueUI(true);
        // PauseController.SetPause(true);

        DisplayCurrentLine();
    }

	private void SyncQuestState()
	{
		if (dialogueData.quest == null) return;
		string questID = dialogueData.quest.questID;

		if (QuestController.Instance.IsQuestCompleted(questID) || QuestController.Instance.IsQuestHandedIn(questID))
		{
			questState = QuestState.Completed;
		}

		else if (QuestController.Instance.IsQuestActive(questID))
		{
			questState = QuestState.InProgress;
		}
		else
		{
			questState = QuestState.NotStarted;
		}

	}

	void NextLine()
	{
		if (isTyping)
		{
			StopAllCoroutines();
			dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);

			isTyping = false;
		}

		//clear out existing choices
		dialogueUI.ClearChoice();

		//check end dialogue lines
		if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
		{
			EndDialogue();
			return;
		}

		//check if choices and display
		foreach(DialogueChoice dialogueChoice in dialogueData.choices)
		{
			if(dialogueChoice.dialogueIndex == dialogueIndex)
			{
				DisplayChoices(dialogueChoice);
				return; //display choices
			}
		}


		if(++dialogueIndex < dialogueData.dialogueLines.Length)
		{
            DisplayCurrentLine();
		}
		else
		{
			EndDialogue();
		}
	}

	IEnumerator TypeLine()
	{
		isTyping = true;
        dialogueUI.SetDialogueText("");

		foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
		{
			dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
		}

		isTyping = false;

		if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
		{
			yield return new WaitForSeconds(dialogueData.autoProgressDelay);
			NextLine();
		}
	}

	void DisplayChoices (DialogueChoice choice)
	{
		for (int i=0; i< choice.choices.Length; i++)
		{
			int nextIndex = choice.nextDialogueIndexes[i];
			bool givesQuest = choice.givesQuest[i];
			dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, givesQuest));
		}
	}

	void ChooseOption (int nextIndex, bool givesQuest)
	{
		if (givesQuest)
		{
			QuestController.Instance.AcceptQuest(dialogueData.quest);
			questState = QuestState.InProgress;
		}

		dialogueIndex = nextIndex;
		dialogueUI.ClearChoice();
		DisplayCurrentLine();

    }

	void DisplayCurrentLine()
	{
		StopAllCoroutines();
		StartCoroutine(TypeLine());
	}

	public void EndDialogue()
	{
		//mark npc as spoken to in dict & feed that info to quest controller
		Debug.Log("EndDialogue called for: " + npcID);
		NPCDictionary.Instance.MarkSpokenTo(npcID);
        QuestController.Instance.CheckNPCsSpokenTo();

        if (questState == QuestState.Completed && !QuestController.Instance.IsQuestHandedIn(dialogueData.quest.questID))
		{
			//handle quest completion
			HandleQuestCompletion(dialogueData.quest);
        }

		StopAllCoroutines();
		isDialogueActive = false;
		dialogueUI.SetDialogueText("");
		dialogueUI.ShowDialogueUI(false);
	}

	//add npc to sleeper list once spoken to. list is used to trigger quest completion
	public void AddToSpokenToList()	{
		//npc will be identified by public string ID
	}

	void HandleQuestCompletion(Quest quest)
	{
		QuestController.Instance.HandinQuest(quest.questID);
	}
}
