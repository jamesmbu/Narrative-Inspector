using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InteractionHandle : MonoBehaviour
{

    private CircleCollider2D TriggerBounds;
    private DialogueHandle DialogueEvent;

    [Tooltip("Specify if this object will initiate a dialogue on interaction\n'DialogueHandle' component will be expected")]
    public bool InitiatesDialogue;

    private bool PlayerInTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        YieldComponents();
        ValidateComponentYields();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerInTrigger)
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
            PlayerInTrigger = false;
            Debug.Log("Player exited trigger of '" + gameObject.name + "'");
        }
    }
    
    void YieldComponents()
    {
        TriggerBounds = GetComponent<CircleCollider2D>();
        DialogueEvent = GetComponent<DialogueHandle>();
    }

    void ValidateComponentYields()
    {
        if (!TriggerBounds)
        {
            Debug.Log(gameObject.name + " has no trigger attached (Missing 'CircleCollider2D')");
        }

        if (!DialogueEvent && InitiatesDialogue)
        {
            Debug.Log(gameObject.name + " has no dialogue component attached (Missing 'DialogueHandle')");
        }
    }

    void PreInteraction()
    {
        if (InitiatesDialogue && DialogueEvent)
        {
            DialogueEvent.TriggerDialogue();
        }
    }
}
