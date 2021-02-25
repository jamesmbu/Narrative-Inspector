using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Attach this component to a game object which is meant to present dialogue on interaction
*/
public class DialogueHandle : MonoBehaviour
{
    public Dialogue dialogue;
    private int progress = 0;

    public void TriggerDialogue()
    {
        if (progress == 0)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            progress++;
        }
        else
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
        
    }
}
