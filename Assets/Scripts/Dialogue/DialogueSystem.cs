using KH;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    #region variabels
    public static DialogueSystem Instance { get; set; }

    public GameObject dialoguePanel;
    public Transform optionsContainer;
    public GameObject optionsButtonPrefab;
    public GameObject nextButtonPrefab;

    public NPC Npc;
    public List<string> dialogueLines = new List<string>();

    Text dialogueText, nameText;
    int dialogueIndex;

    DialogueItem currentDialogue;

    private GameObject nextDialogueButton;

    public InputHandler inputHandler;


    #endregion
    private void Awake()
    {
        #region singleton
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        #endregion

        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<Text>();
        nameText = dialoguePanel.transform.Find("Name").GetChild(0).GetComponent<Text>();
        dialoguePanel.SetActive(false);

    }
    public void AddNewDialogue(string[] lines, string npcName)
    {
        dialogueLines = new List<string>();
        Npc.NPCName = npcName;
        dialogueLines.AddRange(lines);
        currentDialogue = null;
        CreateDialogue();
    }

    #region newDialog with branch
    public void AddNewDialogue(DialogueItem _dialogue, NPC _npc)
    {
        dialogueLines = _dialogue.dialogues;
        currentDialogue = _dialogue;
        Npc = _npc;
        CreateDialogue();
    }
    #endregion

    public void CreateDialogue()
    {
        if (dialogueLines.Count == 0) {
            Debug.LogError("HandledError : dialogueLines is empty");
            return;
        }

        inputHandler.isWindowOpened = true;

        dialoguePanel.SetActive(true);

        dialogueIndex = 0;
        dialogueText.text = dialogueLines[dialogueIndex];
        nameText.text = Npc.name;

        // destroy dialogue branch options
        for (int i = 0; i < optionsContainer.childCount; i++)
            Destroy(optionsContainer.GetChild(i).gameObject);

        if (nextDialogueButton == null) {
            nextDialogueButton = Instantiate(nextButtonPrefab, optionsContainer.parent.parent, false);
            nextDialogueButton.GetComponent<Button>().onClick.AddListener(delegate { ContinueDialogue(); });
        } else {
            nextDialogueButton.SetActive(true);
        }
    }
    public void ContinueDialogue()
    {
        // continue
        if (dialogueIndex < dialogueLines.Count - 1) {
            dialogueIndex++;
            dialogueText.text = dialogueLines[dialogueIndex];

            if (dialogueIndex != dialogueLines.Count - 1)
                return;

            nextDialogueButton.SetActive(false);
            if (currentDialogue?.brancesDialogue.Count > 0) {
                for (int i = 0; i < optionsContainer.childCount; i++)
                    Destroy(optionsContainer.GetChild(i).gameObject);

                var optionCount = 0;
                // generate OptionButton
                foreach (var branchDialogue in currentDialogue.brancesDialogue) {
                    if (branchDialogue == null) {
                        Debug.LogError("Handled Error : You haven't assign the brance story");
                        inputHandler.isWindowOpened = false;
                        dialoguePanel.SetActive(false);
                        Npc.interactableObjectActionContainer.gameObject.SetActive(true);
                        break;
                    }
                    var buttonOptions = Instantiate(optionsButtonPrefab, optionsContainer, false);
                    buttonOptions.GetComponentInChildren<Text>().text = branchDialogue.option;
                    buttonOptions.GetComponent<Button>().onClick.AddListener(delegate { AddNewDialogue(branchDialogue, Npc); });
                    optionCount++;
                }
                StartCoroutine(Delay(.25f));
            } else // done if its doesnt have any branches dialogue
                EndDialogueAction();

        } else
            EndDialogueAction();

    }

    private void EndDialogueAction()
    {
        inputHandler.isWindowOpened = false;
        nextDialogueButton.SetActive(false);
        dialoguePanel.SetActive(false);
        Npc.interactableObjectActionContainer.gameObject.SetActive(true);
        Npc.DialogueFinished();
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        optionsContainer.GetChild(0).gameObject.SetActive(false);
        optionsContainer.GetChild(0).gameObject.SetActive(true);


    }
}
