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
    private DialogueManager dialogueManager;

    [Range(0.0f, 0.1f)] public float typewriterSpeed = 0.02f;

    [HideInInspector]public bool DialogueFinished = false;

    [Tooltip("If true, player will not be able to move whilst dialogue is in progress")]
    public bool WillHaltMovement = true;
    [Tooltip("If true, the dialogue can be cycled through again ")]
    public bool Repeatable = true;

    void Start()
    {
        totalTextBlocks = dialogue.sentences.Length;
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void TriggerDialogue()
    {
        if (progress == 0)
        {
            DialogueFinished = false;
            dialogueManager.StartDialogue(dialogue, typewriterSpeed);
            
            if (dialogueManager.progressionOcurred)
            {
                progress++;
            }

            //Debug.Log(progress);

        }
        else
        {
            dialogueManager.DisplayNextSentence(typewriterSpeed);
            if (dialogueManager.progressionOcurred)
            {
                progress++;
            }
            if (progress > totalTextBlocks)
            {
                DialogueFinished = true;
                if (Repeatable)
                {
                    Debug.Log("Repeating!");
                    progress = 0;
                }
            }
            Debug.Log(progress);
        }
        
    }
}
