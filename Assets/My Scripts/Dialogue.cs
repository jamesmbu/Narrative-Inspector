using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is an object passed into the dialogue manager when we want to start a new dialogue
[System.Serializable]
public class Dialogue
{
    public string name; // such as the name of an NPC
    
    [TextArea(3, 10)]
    public string[] sentences;

    public AudioClip Audio;
}
