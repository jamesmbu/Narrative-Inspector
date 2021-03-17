using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

/*
 * Attach this component to a game object which is meant to present dialogue on interaction
*/
public class DialogueHandle : MonoBehaviour
{
    public Dialogue dialogue;
    
    private int progress = 0;
    private int totalTextBlocks;
    private DialogueManager dialogueManager;
    private bool MultiHandleMode = false;
    public Dialogue[] dialogueMultiHandle;
    private SlideshowManager slideshowManager = null;
    [Range(0.0f, 0.1f)] public float typewriterSpeed = 0.02f;

    [HideInInspector]public bool DialogueFinished = false;

    [Tooltip("If true, player will not be able to move whilst dialogue is in progress")]
    public bool WillHaltMovement = true;
    [Tooltip("If true, the dialogue can be cycled through again ")]
    public bool Repeatable = true;

    private int dialogueGroupTracker = 0;
    void Awake()
    {
        if (dialogueMultiHandle.Length > 0)
        {
            MultiHandleMode = true;
        }
    }

    void Start()
    {
        totalTextBlocks = dialogue.sentences.Length;
        dialogueManager = FindObjectOfType<DialogueManager>();
        slideshowManager = FindObjectOfType<SlideshowManager>();
    }

    public void TriggerDialogue()
    {
        if (MultiHandleMode)
        {
                if (progress == 0)
                {
                    DialogueFinished = false;
                    dialogueManager.StartDialogue(dialogueMultiHandle[dialogueGroupTracker], typewriterSpeed);
                    if (dialogueManager.progressionOcurred)
                    {
                        progress++;
                    }
                }
                else
                {
                    dialogueManager.DisplayNextSentence(typewriterSpeed);
                    if (dialogueManager.progressionOcurred)
                    {
                        progress++;
                    }
                    if (progress > dialogueMultiHandle[dialogueGroupTracker].sentences.Length
                    && dialogueGroupTracker != dialogueMultiHandle.Length-1) // if at the end of a dialogue group
                    {
                        dialogueGroupTracker++;
                        progress = 0;
                        DialogueFinished = true;
                        slideshowManager.Next();
                        TriggerDialogue();
                    }
                }
            
        }
        else if (!MultiHandleMode)
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
                        //Debug.Log("Repeating!");
                        progress = 0;
                    }
                }
                Debug.Log(progress);
            }
        }

        
        
    }
}
