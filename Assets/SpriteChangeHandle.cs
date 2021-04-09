using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChangeHandle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite[] imageArray;

    public bool Loops = false;

    private Sprite[] programImageArray;

    private int tracker = 0;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        /*Make an array based on the designer-visible image array.
         This array will contain the same contents, but with the original image at the first index
        */
        programImageArray = new Sprite[imageArray.Length+1];
        programImageArray[0] = spriteRenderer.sprite;
        for (int i = 0; i < programImageArray.Length; i++)
        {
            if (i == 0) continue;
            programImageArray[i] = imageArray[i-1];
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSprite()
    {
        // goes to next available sprite
        if (tracker < programImageArray.Length-1)
        {
            tracker++;
            spriteRenderer.sprite = programImageArray[tracker];
            
        }
        // if at end of the array, cycles back to beginning
        else
        {
            if (Loops)
            {
                spriteRenderer.sprite = programImageArray[0];
                tracker = 0;
            }
            else
            {
                //spriteRenderer.sprite = programImageArray[0];
            }
            
        }
        
    }
}
