using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
public class VideoEventManager : MonoBehaviour {
    [Header("do not change")]
[SerializeField]
    private Transform camera;
    [SerializeField]
    private Transform menu;
    [SerializeField]
    private Transform MainLogo;
    [SerializeField]
    private GameObject QuitButton;
    [SerializeField]
    private Text MessageField;

    [SerializeField]
    private Transform InAppButtonPrefab;
    [SerializeField]
    private Transform MenuButtonPrefab;
    private VideoPlayer videoPlayer;
    private VideoData.Video currentVideo;

    [SerializeField]
    private Transform buttonHolder;

    [SerializeField]
    private Transform buttonHolderMenu;

    [Header("change data for different experieces")]
    [SerializeField]
    private VideoData[] videoData;
    private VideoData currentVideoData;
    public float FadeTime = 1;
    public float GrayoutTime = 1;

    public enum mode { mobile, Pc}
    public mode currentMode;
    // Use this for initialization
    public UnityEvent OnVideoDoneEvent;

    public bool DontRotateOption = true;
    [Header("debug")]
    public float currentTime = 0;
    public float TimeLeft = 0;
    public float totalTime = 0;

    void Start ()
    {
        videoPlayer = GameObject.FindObjectOfType<VideoPlayer>();
        //buttonHolder = GameObject.FindObjectOfType<ButtonHolder>().transform;
        RenderSettings.skybox.SetFloat("_Exposure", 0);
        QuitButton.GetComponent<Button>().onClick.AddListener(delegate
        {

           Application.Quit();

        });
        
        ShowMainMenu();

    }
    public void ShowMainMenu()
    {
        currentVideo = null;
        MessageField.transform.parent.gameObject.SetActive(false);
        MessageField.transform.parent.GetComponent<ButtonFade>().SetAlpha(0);
        if (RenderSettings.skybox.GetFloat("_Exposure") != 0)
        {
            StartCoroutine(fadeOut(FadeTime));

        }

        if (currentMode == mode.Pc)
        {
            QuitButton.SetActive(true);
        }
        
        MainLogo.gameObject.SetActive(true);
        prepareMenu();
        foreach (VideoData v in videoData)
        {
            Button button = Instantiate(MenuButtonPrefab, buttonHolderMenu).GetComponent<Button>();
            Text t = button.GetComponentInChildren<Text>();
            t.text = v.name;
            button.GetComponent<ButtonFade>().SetAlpha(1);
            if (currentMode == mode.Pc)
            {
                button.GetComponent<ButtonGaze>().enabled = false;
            }
            // Debug.Log(i);
            button.onClick.AddListener(delegate
            {
                //StartCoroutine(fadeIn(FadeTime));
                QuitButton.SetActive(false);
                MainLogo.gameObject.SetActive(false);
                StartVideo(v);

            });
        }
        //Button button2 = Instantiate(ButtonPrefab, buttonHolder).GetComponent<Button>();
        //Text tt = button2.GetComponentInChildren<Text>();
        //tt.text = "quit";
        //button2.GetComponent<ButtonFade>().SetAlpha(1);
        //if (currentMode == mode.Pc)
        //{
        //    button2.GetComponent<ButtonGaze>().enabled = false;
        //}
        //// Debug.Log(i);
        //button2.onClick.AddListener(delegate
        //{

        //    Application.Quit();

        //});
    }

    
    /// <summary>
    /// prepares the video player call backs
    /// 
    /// </summary>
    /// <param name="v"></param>
    public void StartVideo(VideoData v)
    {
        currentVideoData = v;
       // videoDataIndex = i;
       // Debug.Log(i);
        currentVideo = v.video[0];

        videoPlayer.clip = currentVideo.mainClip;
        videoPlayer.loopPointReached += OnVideoDone;
        videoPlayer.playOnAwake = false;

        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += OnVideoPrepared;
        //Play();


        prepareMenu();

    
    }
    public void OnVideoPrepared(VideoPlayer vp)
    {
        videoPlayer.Play();

        MouseMouvement m = FindObjectOfType<MouseMouvement>();

        if (m != null)
        {

            m.OverwriteRotation(currentVideo.YRot);
        }
        if (RenderSettings.skybox.GetFloat("_Exposure") != 1)
        {
            StartCoroutine(fadeIn(FadeTime));

        }
    }

    public void OnDisable()
    {
        RenderSettings.skybox.SetFloat("_Saturation", 1);
        RenderSettings.skybox.SetFloat("_Exposure", 1);

    }
    [ContextMenu("makeDark")]
    public void MakeDark()
    {
        //videoMaterial.SetFloat("_Exposure", 0);
        //  RenderSettings.skybox.SetFloat("_Exposure", 0);
        StartCoroutine(fadeOut(5));
    }
    private void DeleteChildren()
    {

        for (int i  = 0; i < buttonHolder.childCount; i++)
        {
            Destroy(buttonHolder.GetChild(i).gameObject);
        }
        for (int i = 0; i < buttonHolderMenu.childCount; i++)
        {
            Destroy(buttonHolderMenu.GetChild(i).gameObject);
        }
    }
    void prepareMenu()
    {
        menu.rotation = Quaternion.Euler(0, camera.rotation.eulerAngles.y, 0);


        DeleteChildren();
    }
   public VideoData.Video GetVideo(int choice)
    {
        return currentVideoData.video[choice];
    }
    public void SpawnChoices()
    {
        prepareMenu();
        //  for (int i = 0; i < currentVideo.choices.ChoiceClips.Length; i++)
        foreach (VideoData.Choices c in currentVideo.choices)
        {
            Button button = Instantiate(InAppButtonPrefab, buttonHolder).GetComponent<Button>() ;
            Text t = button.GetComponentInChildren<Text>();
            t.text = c.ChoiceText;
            button.GetComponent<ButtonFade>().SetAlpha(0);

            if(currentMode == mode.Pc)
            {
                button.GetComponent<ButtonGaze>().enabled = false;
            }
           // Debug.Log(i);
            button.onClick.AddListener(delegate 
            {
                currentMessageIndex = 0;
                MessageField.transform.parent.GetComponent<ButtonFade>().SetAlpha(0);

                MessageField.transform.parent.gameObject.SetActive(false);
                if (c.ChoiceClips == -1)
                {
                    ShowMainMenu();
                    StartCoroutine(HideOverlay(GrayoutTime)); 
                }
             
                else
                { 
                    SetVideo(GetVideo(c.ChoiceClips));
                    Play();
                    StartCoroutine( HideOverlay(GrayoutTime));
                    DeleteChildren();
                }
            });
        }
    }
	public void OnVideoDone(VideoPlayer vp)
    {
        SpawnChoices();

        StartCoroutine( ShowOverlay(GrayoutTime));
        StartCoroutine(FadeInButton(GrayoutTime));

        //  OnVideoDoneEvent.Invoke();
        Debug.Log("video done");
       
    }

    public IEnumerator FadeInButton(float time = 1)
    {
        float elapsedTime = 0;
        float Saturation = 0;
        ButtonFade[] fades = buttonHolder.GetComponentsInChildren<ButtonFade>();
        while (elapsedTime < time)
        {
            Saturation = (elapsedTime / time);
            foreach (ButtonFade f in fades)
            {
                f.SetAlpha(Saturation);
            }
           
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();

        }

    }


    /// <summary>
    /// desaturizes the game view
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator ShowOverlay(float time = 1)
    {
        float elapsedTime = 0;
        float Saturation = 1;
        while (elapsedTime < time)
        {
            Saturation = 1 - (elapsedTime / time);
            RenderSettings.skybox.SetFloat("_Saturation", Saturation);
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();

        }

    }
    public IEnumerator HideOverlay(float time = 1)
    {
        float elapsedTime = 0;
        float Saturation = 0;
        while (elapsedTime < time)
        {
            Saturation = (elapsedTime / time);
            RenderSettings.skybox.SetFloat("_Saturation", Saturation);
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();

        }
    }
   
    public void SetVideo(VideoData.Video video)
    {
        currentVideo = video;
        
    }
        
    public void Play()
    {
        StartCoroutine(SwitchVideoClip());
       // videoPlayer.clip = currentVideo.mainClip;
      //  videoPlayer.Pause();
    //    videoPlayer.Play();
    }
    public IEnumerator SwitchVideoClip()
    {
        videoPlayer.clip = currentVideo.mainClip;
        videoPlayer.Prepare();
        yield return StartCoroutine(fadeOut(FadeTime));
        Debug.Log(RenderSettings.skybox.GetFloat("_Exposure"));
        
        videoPlayer.Play();
        yield return new WaitForEndOfFrame();

      //   videoPlayer.Pause();
        Debug.Log(RenderSettings.skybox.GetFloat("_Exposure"));

        yield return StartCoroutine(fadeIn(FadeTime));

       // videoPlayer.Play();

    }
    private IEnumerator fadeIn(float time)
    {

        float elapsedTime = 0;
        float exposure = 0;
        while (elapsedTime < time)
        {
            exposure = (elapsedTime / time);
            RenderSettings.skybox.SetFloat("_Exposure", exposure);
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();

        }

    }
    private IEnumerator fadeOut(float time)
    {

        float elapsedTime = 0;
        float exposure = 1;
        while (elapsedTime < time)
        {
            exposure = 1 - (elapsedTime / time);
            RenderSettings.skybox.SetFloat("_Exposure", exposure);
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();

        }
       
    }
    public void PlayNextVideo(VideoClip clip)
    {
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }

    public int currentMessageIndex = 0;
	// Update is called once per frame
	void Update ()
    {
        //  Debug.Log(videoPlayer.time);
        if (currentVideo != null)
        {
            currentTime = (float)videoPlayer.time;
            totalTime = (float)videoPlayer.clip.length;
            TimeLeft = totalTime - currentTime;
        }
     
        if (currentVideo != null && currentVideo.Message.messages.Length > 0 && currentMessageIndex < currentVideo.Message.messages.Length)
        {
            
            if(videoPlayer.time > videoPlayer.clip.length - GetMessageDurationAfterIndex(currentMessageIndex))
            {
                if (MessageField.color.a  != 1 )
                {
                    MessageField.transform.parent.GetComponent<ButtonFade>().justText = false;
                    MessageField.transform.parent.gameObject.SetActive(true);
                    menu.rotation = Quaternion.Euler(0, camera.rotation.eulerAngles.y, 0);

                }
                else
                {
                    MessageField.transform.parent.GetComponent<ButtonFade>().justText = true;

                }
                StartCoroutine(FadeInMessageBox(GrayoutTime));

                // Debug.Log("dopio");
                MessageField.text = currentVideo.Message.messages[currentMessageIndex].MessageText;
                //if(currentMessageIndex + 1 < currentVideo.Message.messages.Length)
                    currentMessageIndex++;
                
            }
        }
        else
        {
         //   MessageField.transform.parent.gameObject.SetActive(false);

        }

    }
    public IEnumerator FadeInMessageBox(float time = 1)
    {
        float elapsedTime = 0;
        float Saturation = 0;

        ButtonFade fade = MessageField.transform.parent.GetComponent<ButtonFade>();
        Debug.Log(fade);
        while (elapsedTime < time)
        {
            Saturation = (elapsedTime / time);
            //foreach (ButtonFade f in fades)
          //  {
              //  Debug.Log("solo");
                fade.SetAlpha(Saturation);
           // }

            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();

        }
        fade.SetAlpha(1);

    }
    public float GetMessageDurationAfterIndex(int Index)
    {
        float durationFromCurrentIndex = 0f;
        for (int i = currentMessageIndex; i < currentVideo.Message.messages.Length; i++)
        {
            durationFromCurrentIndex += currentVideo.Message.messages[i].timeOnScreen;
        }

        return durationFromCurrentIndex;
    }
}
