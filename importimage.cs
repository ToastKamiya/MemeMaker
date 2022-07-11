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
using TMPro;
// using Unity Standalone File Browser by gkngkc on github
// github.com/gkngkc/UnityStandaloneFileBrowser
 
public class importimage : MonoBehaviour
{
 
    public GameObject mainImg;
    public GameObject tutorialText2;
    public GameObject PopupMenuGO;

    public Text tutorialText;

    public TMP_Text TopTMP;
    public TMP_Text BottomTMP;

    public TMP_InputField RightIF;
    public TMP_InputField LeftIF;

    public bool texthidden; // really don't need this but this way there are no errors being thrown that might mess with performance

    public RectTransform RT;

    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;

	void Start() 
	{
		InvokeRepeating("Process", 0, 5f);
		StartCoroutine(FadeTextToZeroAlpha(1f, tutorialText));
	}

	public void Process()
    {
        ScreenCapture.CaptureScreenshot(Directory.GetCurrentDirectory() + "/" + "GeneratedMeme.png");
    }

    public void StartProcessIE()
    {
        StartCoroutine(ProcessIE()); // I know how stupid this is but this is the only way to start an IEnumerator using unity button actions
    }

    public void StartSaveToIE()
    {
        StartCoroutine(SaveToIE()); // I know how stupid this is but this is the only way to start an IEnumerator using unity button actions
    }

    IEnumerator ProcessIE() // cause unity buttons can't invoke IEnumerators, only voids, which you can use to invoke IEnumerators
    {
        yield return new WaitForSeconds(0.5f);
        ScreenCapture.CaptureScreenshot(Directory.GetCurrentDirectory() + "/" + "GeneratedMeme.png");
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PopupMenu();
        }
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(HidePopupMenu());
        }
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.Return))
        {
            ScreenCapture.CaptureScreenshot(Directory.GetCurrentDirectory() + "/" + "GeneratedMeme.png");
        }

        if (Input.GetMouseButtonDown(0))
        {
            clicked++;
                if (clicked == 1) clicktime = Time.time;
     
                if (clicked > 1 && Time.time - clicktime < clickdelay)
                {
                    clicked = 0;
                    clicktime = 0;
                    OpenImage();
                }

            else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
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

    public void SetTopTextFontSize()
    {
        TopTMP.fontSize = float.Parse(LeftIF.text);
    }

    void PopupMenu()
    {
        PopupMenuGO.SetActive(true);
        RT.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void OpenImage()
    {
        if(!texthidden)
        {
            tutorialText2.SetActive(false);
            texthidden = true;
        }
        var paths = StandaloneFileBrowser.OpenFilePanel("Select image", "", "", false);
        if (paths.Length > 0) 
        {
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        }
    }

    public IEnumerator SaveToIE()
    {
        yield return new WaitForSeconds(0.5f);
        var path = StandaloneFileBrowser.SaveFilePanel("Title", "", "GeneratedMeme.png", "");
        if (!string.IsNullOrEmpty(path)) {
            ScreenCapture.CaptureScreenshot(path);
        }
    }

    public IEnumerator HidePopupMenu()
    {
        yield return new WaitForSeconds(0.05f);
        PopupMenuGO.SetActive(false);
        RT.anchoredPosition = new Vector2(9999, 9999);
    }

    public void SetBottomTextFontSize()
    {
        BottomTMP.fontSize = float.Parse(RightIF.text);
    }

    public void OpenGithub()
    {
        Application.OpenURL("https://github.com/ToastKamiya/MemeMaker");
    }

    public void OpenComplaints()
    {
        Application.OpenURL("https://github.com/ToastKamiya/MemeMaker/issues");
    }

}
