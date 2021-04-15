using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class SlideshowManager : MonoBehaviour
{
    public Sprite[] PhotoBook;

    [Tooltip("Reference to the UI element of type Image which displays the images of the slideshow")]
    public Image SlideshowImageUI;

    private int currentImageIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (SlideshowImageUI)
        {
            SlideshowImageUI.GetComponent<Image>().sprite = PhotoBook[currentImageIndex];
        }
    }

    public void Next()
    {
        if (SlideshowImageUI)
        {
            //Debug.Log("next");
            currentImageIndex++;
            SlideshowImageUI.GetComponent<Image>().sprite = PhotoBook[currentImageIndex];
        }
    }

    public void Previous()
    {
        if (SlideshowImageUI)
        {
            SlideshowImageUI.GetComponent<Image>().sprite = PhotoBook[currentImageIndex--];
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
