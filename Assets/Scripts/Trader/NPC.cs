using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public Button shopButton;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private bool isLastLine = false; // Track if we're at last line

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseController.isGamePaused && !isDialogueActive))
            return;

        if (isDialogueActive)
        {
            // Handle special case for last line
            if (isLastLine && !isTyping)
            {
                EndDialogue();
                return;
            }
            
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
        isLastLine = false;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        shopButton.gameObject.SetActive(false); // Hide button initially
        PauseController.SetPause(true);

        StartCoroutine(TypeRoutine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            
            // Check if we should show shop button after skipping typing
            CheckForShopButton();
            return;
        }

        dialogueIndex++;
        
        if (dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeRoutine());
        }
        else
        {
            // Prepare to end dialogue but show shop button first
            isLastLine = true;
            shopButton.gameObject.SetActive(true);
            shopButton.interactable = true;
        }
    }

    IEnumerator TypeRoutine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        CheckForShopButton();

        // Handle auto-progress
        if (dialogueData.autoProgressLines.Length > dialogueIndex && 
            dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            
            if (dialogueIndex < dialogueData.dialogueLines.Length - 1)
            {
                dialogueIndex++;
                StartCoroutine(TypeRoutine());
            }
            else
            {
                isLastLine = true;
                shopButton.gameObject.SetActive(true);
                shopButton.interactable = true;
            }
        }
    }

    void CheckForShopButton()
    {
        // Show shop button if at last line and not typing
        if (dialogueIndex == dialogueData.dialogueLines.Length - 1 && !isTyping)
        {
            isLastLine = true;
            shopButton.gameObject.SetActive(true);
            shopButton.interactable = true;
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        isLastLine = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        shopButton.gameObject.SetActive(false); // Hide button
        PauseController.SetPause(false);
    }

    // Call this from your shop button's OnClick event
    public void OpenShop()
    {
        Debug.Log("Shop opened!");
        // Implement your shop opening logic here
        EndDialogue(); // Close dialogue when shop opens
    }
}