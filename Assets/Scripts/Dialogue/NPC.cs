

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NPC : Interactable
{
    public string NPCName;
    public DialogueItem dialogueSO;

    private Animator npcAnimator;

    /// <summary>
    /// This Start() is virtual because it is assign npcAnimator for dialogue animation, and should be call by the heir e.g QuestGiver
    /// </summary>
    public virtual void Start()
    {
        npcAnimator = GetComponentInChildren<Animator>();
    }
    /// <summary>
    /// This is Interact() on NPC class
    /// </summary>
    public override void Interact()
    {
        // NPC looking to Interactor Object
        StartCoroutine(LookAtInteractedOnject(currentTransform: this.transform, lookTransform: player, rotationTime: .8f));
        // Interactor looking at NPC
        StartCoroutine(LookAtInteractedOnject(currentTransform: player, lookTransform: this.transform, rotationTime: .8f));

        DialogueSystem.Instance.AddNewDialogue(dialogueSO, this);

        if (instantiatedPrefab != null)
            interactableObjectActionContainer.gameObject.SetActive(false);

    }

    IEnumerator LookAtInteractedOnject(Transform currentTransform, Transform lookTransform, float rotationTime)
    {
        Vector3 direction = (lookTransform.position - currentTransform.position).normalized;
        ;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        while (rotationTime >= 0) {
            yield return new WaitForSeconds(Time.deltaTime);
            rotationTime -= .1f;
            currentTransform.rotation = Quaternion.Slerp(currentTransform.rotation, lookRotation, Time.deltaTime * 5);
        }
        currentTransform.rotation = lookRotation;

        npcAnimator.SetBool("isInteracting", true);
        npcAnimator.CrossFade("Talking_1", .5f);
    }

    /// <summary>
    /// This function used for reset npc animation to locomotion, and called from PlayerDialogueHandler when dialogue finished
    /// </summary>
    public void DialogueFinished()
    {
        npcAnimator.SetBool("isInteracting", false);
    }
}
