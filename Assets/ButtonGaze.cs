using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGaze : MonoBehaviour {
    public float GazeTime = 1;
    private float GazeTimer = 1;
    public Image Image;
    private bool gaze = false;
    public void OnStartGaze()
    {
        GazeTimer = 0;

        gaze = true;
    }
    public void OnGaze()
    {
        Image.fillAmount = GazeTimer / GazeTime;
        GazeTimer += Time.deltaTime;
        if(GazeTimer > GazeTime)
        {
            this.GetComponent<Button>().onClick.Invoke();
        }
    }
    private void Update()
    {
        if(gaze)
            OnGaze();
    }
    public void OnEndGaze()
    {
        Image.fillAmount = 0;
        gaze = false;
    }

}
