using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class to give animation on dialogue text when its continue by call ContinueNextDialogue() from animationEvent
/// </summary>
public class ContinueDialogue : MonoBehaviour
{
    public string dialogueText;
    private Text text;
    private void Start()
    {
        text = GetComponent<Text>();
    }
    public void ContinueNextDialogue()
    {
        text.text = dialogueText;// tampilkan di UI
    }

    public void PopUpOption()
    {

    }
}
