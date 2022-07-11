using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using System;
using System.IO;
// using Unity Standalone File Browser by gkngkc on github
// github.com/gkngkc/UnityStandaloneFileBrowser
 
public class importimage : MonoBehaviour, IPointerClickHandler
{
 
    public GameObject mainImg;

    public Text tutorialText;
    public GameObject tutorialText2;

    public bool texthidden; // really don't need this but this way there are no errors being thrown that might mess with performance

	void Start() 
	{
		InvokeRepeating("Process", 0, 5f);
		StartCoroutine(FadeTextToZeroAlpha(1f, tutorialText));
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        int clickCount = eventData.clickCount;
		if (clickCount == 2)
		{
            if(!texthidden)
            {
                tutorialText2.SetActive(false);
                texthidden = true;
            }
			var paths = StandaloneFileBrowser.OpenFilePanel("Select image", "", "", false);
        	if (paths.Length > 0) {
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        	}
		}
            
    }

	void Process()
    {
        ScreenCapture.CaptureScreenshot(Directory.GetCurrentDirectory() + "/" + "GeneratedMeme.png");
    }

    void Update()
    {
    	if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.Return))
    	{
    		Process();
    	}
    }

    private IEnumerator OutputRoutine(string url) {
        var loader = new WWW(url);
        yield return loader;
        mainImg.GetComponent<RawImage>().texture = loader.texture;
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

}
