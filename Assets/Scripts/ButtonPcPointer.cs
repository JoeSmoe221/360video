using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPcPointer : MonoBehaviour
{
    VideoEventManager manager;
    GlobalSettingsManager cursorManager;
    public void Start()
    {
        manager = FindObjectOfType<VideoEventManager>();
        cursorManager = FindObjectOfType<GlobalSettingsManager>();
    }
    public void onEnter()
    {
        if(manager.currentMode == VideoEventManager.mode.Pc)
        {
            cursorManager.SetClicker();
        }

    }
    public void onExit()
    {
        if (manager.currentMode == VideoEventManager.mode.Pc)
        {
            cursorManager.SetHand();
        }
    }
}
