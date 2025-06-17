using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;

public class BossDialogue : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public GameObject mainGem;


    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private bool isLastLine = false; // Track if we're at last line

    public bool InteractReady = false;

    public bool CanInteract()
    {
        return !isDialogueActive && InteractReady;
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
            }
        }
    }

    void CheckForShopButton()
    {
        // Show shop button if at last line and not typing
        if (dialogueIndex == dialogueData.dialogueLines.Length - 1 && !isTyping)
        {
            isLastLine = true;
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        isLastLine = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        PauseController.SetPause(false);

        if (mainGem)
        {
            GameObject droppedItem = Instantiate(mainGem, transform.position+ new Vector3(0f,-5f,0f) + Vector3.down, Quaternion.identity);
            droppedItem.GetComponent<Bounce>().StartBounce();
            InteractReady = false;
        }
    }

    // Call this from your shop button's OnClick event
    public void OpenShop()
    {
        Debug.Log("Shop opened!");
        // Implement your shop opening logic here
        EndDialogue(); // Close dialogue when shop opens
    }
}