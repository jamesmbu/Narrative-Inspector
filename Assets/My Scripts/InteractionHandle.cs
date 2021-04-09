using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;
using UnityEngine.XR;

public class InteractionHandle : MonoBehaviour
{

    private CircleCollider2D TriggerBounds;

    /* Appearance ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    [Tooltip("Specify if this object's sprite changes on interaction\n'SpriteChangeHandle' component will be expected")]
    public bool SpriteChangesOnInteraction;
    private SpriteChangeHandle spriteChangeHandle;
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    /* Dialogue ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    [Tooltip("Specify if this object will initiate a dialogue on interaction\n'DialogueHandle' component will be expected")]
    public bool InitiatesDialogue;
    private DialogueManager dialogueManager;
    private DialogueHandle DialogueEvent;
    [Tooltip("If false, dialogue will be started by the scene director of the timeline manager")]
    public bool PlayerControlsDialogue = true;
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    private bool PlayerInTrigger = false;

    public GameObject playerController = null;

    [Tooltip("If true, interaction requires no trigger event")]
    public bool NoTriggerRequired = false;

    void Awake()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<DialogueManager>();
        playerController = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        YieldComponents();
        
        //ValidateComponentYields();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerControlsDialogue && (PlayerInTrigger || NoTriggerRequired))
        {
            PreInteraction();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInTrigger = true;
            //Debug.Log("Player entered trigger of '" + gameObject.name + "'");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player exited trigger of '" + gameObject.name + "'");
            PlayerInTrigger = false;
            
        }
    }
    
    void YieldComponents()
    {
        if (!NoTriggerRequired)
            TriggerBounds = GetComponent<CircleCollider2D>();
        DialogueEvent = GetComponent<DialogueHandle>();
        if (SpriteChangesOnInteraction)
        {
            spriteChangeHandle = GetComponent<SpriteChangeHandle>();
        }
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
    public void PreInteraction()
    {
        if (SpriteChangesOnInteraction)
        {
            spriteChangeHandle.UpdateSprite();
        }
        // Dialogue
        if (DialogueEvent)
        {
            PlayerControlsDialogue = true; //to allow for further dialogue to be player controlled
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
