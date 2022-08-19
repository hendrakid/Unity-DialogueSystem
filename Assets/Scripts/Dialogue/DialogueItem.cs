using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class DialogueItem : ScriptableObject
{
    public string option;
    public List<string> dialogues;

    public List<DialogueItem> brancesDialogue;

}
