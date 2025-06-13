
using System.Collections;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        Debug.Log($"Interact called. isDialogueActive = {isDialogueActive}, index = {dialogueIndex}");

        if (dialogueData == null || (PauseController.isGamePaused && !isDialogueActive))
            return;

        if (isDialogueActive)
        {
            Debug.Log("NextLine called from Interact()");
            NextLine();
        }
        else
        {
            Debug.Log("Starting dialogue...");
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        PauseController.SetPause(true);

        //Typeline
        StartCoroutine(TypeRoutine());
    }

    private IEnumerator TypeRoutine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueIndex < dialogueData.dialogueLines.Length - 1 // not last line
            && dialogueData.autoProgressLines.Length > dialogueIndex
            && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            Debug.Log("Typing skipped.");
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            Debug.Log($"Next line: {dialogueIndex}");
            StartCoroutine(TypeRoutine());
        }
        else
        {
            Debug.Log("Reached end of dialogue.");
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        if (!isDialogueActive) return;

        Debug.Log("Ending dialogue"); // <-- add this line

        isDialogueActive = false;
        StopAllCoroutines();

        if (dialogueText != null)
            dialogueText.SetText("");

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        PauseController.SetPause(false);
    }
    
}

// Code references:
// 1) The NPC Dialogue and etc...:https://www.youtube.com/watch?v=eSH9mzcMRqw