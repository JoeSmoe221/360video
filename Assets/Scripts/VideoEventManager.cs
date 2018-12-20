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
    private Transform ButtonPrefab;
    private VideoPlayer videoPlayer;
    private VideoData.Video currentVideo;
    private Transform buttonHolder;
[Header("change data for different experieces")]
    [SerializeField]
    private VideoData[] videoData;
    private VideoData currentVideoData;
    public float FadeTime = 1;
    public float GrayoutTime = 1;

    // Use this for initialization
    public UnityEvent OnVideoDoneEvent;
	void Start () {
        videoPlayer = GameObject.FindObjectOfType<VideoPlayer>();
        buttonHolder = GameObject.FindObjectOfType<ButtonHolder>().transform;

        ShowMainMenu();

    }
    public void ShowMainMenu()
    {
        prepareMenu();
        foreach (VideoData v in videoData)
        {
            Button button = Instantiate(ButtonPrefab, buttonHolder).GetComponent<Button>();
            Text t = button.GetComponentInChildren<Text>();
            t.text = v.name;
            button.GetComponent<ButtonFade>().SetAlpha(1);
            // Debug.Log(i);
            button.onClick.AddListener(delegate
            {

                StartVideo(v);

            });
        }
        Button button2 = Instantiate(ButtonPrefab, buttonHolder).GetComponent<Button>();
        Text tt = button2.GetComponentInChildren<Text>();
        tt.text = "quit";
        button2.GetComponent<ButtonFade>().SetAlpha(1);
        // Debug.Log(i);
        button2.onClick.AddListener(delegate
        {

            Application.Quit();

        });
    }
    public void StartVideo(VideoData v)
    {
        currentVideoData = v;
       // videoDataIndex = i;
       // Debug.Log(i);
        currentVideo = v.video[0];

        videoPlayer.clip = currentVideo.mainClip;
        videoPlayer.loopPointReached += OnVideoDone;
        videoPlayer.playOnAwake = false;
        videoPlayer.Play();
        prepareMenu();
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
            Button button = Instantiate(ButtonPrefab, buttonHolder).GetComponent<Button>() ;
            Text t = button.GetComponentInChildren<Text>();
            t.text = c.ChoiceText;
            button.GetComponent<ButtonFade>().SetAlpha(0);
           // Debug.Log(i);
            button.onClick.AddListener(delegate 
            {
                if(c.ChoiceClips == -1)
                {
                    ShowMainMenu();
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
	// Update is called once per frame
	void Update () {
		
	}
}
