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

    private Queue<string> sentences;

    private TopDownCharacterController playerController;
    
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownCharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
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
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Initiating dialogue with " + dialogue.name);
        SetDialogueBoxVisibility(true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        Debug.Log("End of dialogue");
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
