using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class SlideshowManager : MonoBehaviour
{
    public Sprite[] PhotoBook;

    [Tooltip("Reference to the UI element of type Image which displays the images of the slideshow")]
    public Image SlideshowImageUI;

    // Start is called before the first frame update
    void Start()
    {
        if (SlideshowImageUI)
        {
            SlideshowImageUI.GetComponent<Image>().sprite = PhotoBook[Random.Range(0,3)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
