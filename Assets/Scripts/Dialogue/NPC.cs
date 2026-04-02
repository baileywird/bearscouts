using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
	public NPCDialogue dialogueData;
	public GameObject dialoguePanel;
	public TMP_Text dialogueText, nameText;
	public Image portraitImage;
	public int ID;

	private DialogueController dialogueUI;
	private int dialogueIndex;
	private bool isTyping, isDialogueActive;


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
		isDialogueActive = true;
		dialogueIndex = 0;

		dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
		dialogueUI.ShowDialogueUI(true);
        // PauseController.SetPause(true);

        DisplayCurrentLine();
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
			dialogueUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
		}
	}

	void ChooseOption (int nextIndex)
	{
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
		StopAllCoroutines();
		isDialogueActive = false;
		dialogueUI.SetDialogueText("");
		dialogueUI.ShowDialogueUI(false);
		// PauseController.SetPause(false);
	}
}
