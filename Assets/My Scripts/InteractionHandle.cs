using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandle : MonoBehaviour
{

    private CircleCollider2D TriggerBounds;

    // Start is called before the first frame update
    void Start()
    {
        TriggerBounds = GetComponent<CircleCollider2D>();
        if (!TriggerBounds)
        {
            Debug.Log(gameObject.name + " has no trigger attached (Missing CircleCollider2D)");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger of '" + gameObject.name + "'");
        }
    }
}
