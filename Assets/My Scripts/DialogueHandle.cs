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
    private int totalTextBlocks;
    
    [HideInInspector]public bool DialogueFinished = false;

    [Tooltip("If true, player will not be able to move whilst dialogue is in progress")]
    public bool WillHaltMovement = true;
    [Tooltip("If true, the dialogue can be cycled through again ")]
    public bool Repeatable = true;

    void Start()
    {
        totalTextBlocks = dialogue.sentences.Length;
    }

    public void TriggerDialogue()
    {
        if (progress == 0)
        {
            DialogueFinished = false;
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            progress++;
        }
        else
        {

            FindObjectOfType<DialogueManager>().DisplayNextSentence();
            progress++;
            if (progress > totalTextBlocks)
            {
                DialogueFinished = true;
                if (Repeatable)
                {
                    progress = 0;
                }
            }
        }
        
    }
}
