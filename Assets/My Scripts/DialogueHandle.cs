using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

/*
 * Attach this component to a game object which is meant to present dialogue on interaction
*/
public class DialogueHandle : MonoBehaviour
{
    public ObjectiveTracker ObjectiveTracker;
    public SceneChanger Transition;
    public Dialogue dialogue;
    private AudioSource AudioPlayer;
    private int progress = 0;
    private int totalTextBlocks;
    private DialogueManager dialogueManager;
    private bool MultiHandleMode = false;
    public Dialogue[] dialogueMultiHandle;
    private SlideshowManager slideshowManager = null;
    [Range(0.0f, 0.1f)] public float typewriterSpeed = 0.03f;

    [HideInInspector]public bool DialogueFinished = false;

    [Tooltip("If true, player will not be able to move whilst dialogue is in progress")]
    public bool WillHaltMovement = true;
    [Tooltip("If true, the dialogue can be cycled through again ")]
    public bool Repeatable = true;

    public int dialogueGroupTracker = 0;
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
        AudioPlayer = GetComponent<AudioSource>();
        ObjectiveTracker = FindObjectOfType<ObjectiveTracker>();
    }

public void TriggerDialogue()
    {
        if (MultiHandleMode)
        {
                if (progress == 0)
                {
                    DialogueFinished = false;
                    
                    // Initiate dialogue
                    dialogueManager.StartDialogue(dialogueMultiHandle[dialogueGroupTracker], typewriterSpeed);
                    if (dialogueManager.progressionOcurred)
                    {
                        progress++;
                    }

                    // Check if there is an associated audio. If so, play it.
                    if (dialogueMultiHandle[dialogueGroupTracker].Audio != null)
                    {
                        AudioPlayer.clip = dialogueMultiHandle[dialogueGroupTracker].Audio;
                        AudioPlayer.Play();
                    }

                    // Check if this dialogue counts towards the objectives list. If so, tally it.
                    if (dialogueMultiHandle[dialogueGroupTracker].IsObjective)
                    {
                        dialogueMultiHandle[dialogueGroupTracker].IsObjective = false;
                        ObjectiveTracker.TallyObjective();
                    }
                }
                // If a block of dialogue has already been started i.e., it is in progress...
                else
                {
                    // Display the next sentence of the sequence, increment the progress tracker
                    dialogueManager.DisplayNextSentence(typewriterSpeed);
                    if (dialogueManager.progressionOcurred)
                    {
                        progress++;
                    }
                    // If at the end of a dialogue group, but not the final dialogue group of the sequence...
                    if (progress > dialogueMultiHandle[dialogueGroupTracker].sentences.Length 
                        && dialogueGroupTracker != dialogueMultiHandle.Length-1) 
                    {
                        CheckEndSceneHandle();
                        dialogueGroupTracker++;
                        progress = 0;
                        DialogueFinished = true;
                        
                        if (slideshowManager) slideshowManager.Next();
                        TriggerDialogue();
                        
                    }
                    // If at the end of the final dialogue group
                    else if (progress > dialogueMultiHandle[dialogueGroupTracker].sentences.Length
                        && dialogueGroupTracker == dialogueMultiHandle.Length - 1) // if at the end of a dialogue group
                    {
                        CheckEndSceneHandle();
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

                if (dialogue.Audio)
                {
                    AudioPlayer.clip = dialogue.Audio;
                    AudioPlayer.Play();
                }
                if (dialogue.IsObjective)
                {
                    dialogue.IsObjective = false;
                    ObjectiveTracker.TallyObjective();
                }
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
                else if (dialogue.TriggersEndScene && Transition)
                {
                    Transition.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }

        // Function to check if a dialogue group is a direct precursor to a scene transition. If so, the transition is handled
        void CheckEndSceneHandle()
        {
            if (dialogueMultiHandle[dialogueGroupTracker].TriggersEndScene && Transition)
            {
                Transition.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
