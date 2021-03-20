using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Image dialogueBox;
    public bool expectsPlayer = true;

    private Queue<string> sentences;

    private TopDownCharacterController playerController;
    [HideInInspector] public bool typingInProgress = false;
    [HideInInspector] public bool progressionOcurred = true;
    private string dialogueBlockTemp;
    private AudioSource AudioPlayer;

    void Awake()
    {
        if (expectsPlayer)
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownCharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        AudioPlayer = GetComponent<AudioSource>();
        ValidateTextUI();
        SetDialogueBoxVisibility(false);
    }

    public void SetDialogueBoxVisibility(bool visible)
    {
        Vector3 temp = new Vector3(0.0f, 1000.0f, 0.0f);
        if (visible)
            dialogueBox.transform.localPosition += temp;
        else if (!visible)
            dialogueBox.transform.localPosition -= temp;
    }
    public void StartDialogue(Dialogue dialogue, float speed)
    {
        Debug.Log("Initiating dialogue with " + dialogue.name);
        SetDialogueBoxVisibility(true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence(speed);
    }

    public void DisplayNextSentence(float typewriterSpeed)
    {
        if (sentences.Count == 0 && !typingInProgress)
        {
            EndDialogue();
            progressionOcurred = true;
            return;
        }

        if (typingInProgress)
        {
            progressionOcurred = false;
            StopAllCoroutines();
            dialogueText.text = dialogueBlockTemp;
            typingInProgress = false;
        }
        else
        {
            string sentence = sentences.Dequeue();
            progressionOcurred = true;
            StartCoroutine(Typewrite(sentence, typewriterSpeed));
        }
        
    }

    IEnumerator Typewrite(string sentence, float speed)
    {
        typingInProgress = true;
        dialogueText.text = "";
        dialogueBlockTemp = sentence;
        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            AudioPlayer.Play();
            yield return new WaitForSeconds(speed);
            
        }

        typingInProgress = false;
        //Debug.Log("Finished coroutine");
    }

    void EndDialogue()
    {
       // Debug.Log("End of dialogue");
        SetDialogueBoxVisibility(false);
    }

    void ValidateTextUI()
    {
        if (!dialogueText)
        {
            Debug.Log(gameObject.name + " is missing a reference to UI Text element. See the Canvas prefab to find a reference to a Text element.");
        }

        if (!nameText)
        {
            Debug.Log(gameObject.name + " is missing a reference to UI Text element. See the Canvas prefab to find a reference to a Text element.");
        }
    }

}
