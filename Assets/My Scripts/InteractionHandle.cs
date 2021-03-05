using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;
using UnityEngine.XR;

public class InteractionHandle : MonoBehaviour
{

    private CircleCollider2D TriggerBounds;
    private DialogueHandle DialogueEvent;

    [Tooltip("Specify if this object will initiate a dialogue on interaction\n'DialogueHandle' component will be expected")]
    public bool InitiatesDialogue;

    private bool PlayerInTrigger = false;

    private DialogueManager dialogueManager;

    public GameObject playerController = null;

    [Tooltip("If true, interaction requires no trigger event")]
    public bool NoTriggerRequired = false;

    void Awake()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<DialogueManager>();
        //playerController = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        YieldComponents();
        ValidateComponentYields();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (PlayerInTrigger || NoTriggerRequired))
        {
            PreInteraction();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInTrigger = true;
            Debug.Log("Player entered trigger of '" + gameObject.name + "'");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger of '" + gameObject.name + "'");
            PlayerInTrigger = false;
            
        }
    }
    
    void YieldComponents()
    {
        if (!NoTriggerRequired)
            TriggerBounds = GetComponent<CircleCollider2D>();
        DialogueEvent = GetComponent<DialogueHandle>();
    }

    void ValidateComponentYields()
    {
        if (!TriggerBounds && !NoTriggerRequired)
        {
            Debug.Log(gameObject.name + " has no trigger attached (Missing 'CircleCollider2D')");
        }

        if (!DialogueEvent && InitiatesDialogue)
        {
            Debug.Log(gameObject.name + " has no dialogue component attached (Missing 'DialogueHandle')");
        }
    }

    // Checks which interactions are expected to occur and if the proper references are present.
    void PreInteraction()
    {
        // Dialogue
        if (InitiatesDialogue && DialogueEvent)
        {
            SetPlayerMovement(false);
            DialogueEvent.TriggerDialogue();
            if (DialogueEvent.DialogueFinished) // detect when the dialogue is over
            {
                SetPlayerMovement(true);
            }
        }
        // Other (...)
    }

    void SetPlayerMovement(bool canMove)
    {
        if (NoTriggerRequired) return;

        if (!canMove && DialogueEvent.WillHaltMovement)
            playerController.GetComponent<TopDownCharacterController>().enabled = false;
        else
        {
            playerController.GetComponent<TopDownCharacterController>().enabled = true;
        }
    }
}
