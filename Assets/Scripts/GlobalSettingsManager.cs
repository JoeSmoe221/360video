using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalSettingsManager : MonoBehaviour
{

    CursorMode cursorMode = CursorMode.Auto;

    public Texture2D handCursorTexture;
    public Texture2D grabCursorTexture;
    public Texture2D ClickCursorTexture;

    //public CursorMode cursorMode = CursorMode.;
    // Use this for initialization
    void Start()
    {
        Cursor.SetCursor(handCursorTexture, Vector2.zero, cursorMode);

    }
    public void SetClicker()
    {
        Cursor.SetCursor(ClickCursorTexture, Vector2.zero, cursorMode);

    }
    public void SetHand()
    {
        Cursor.SetCursor(handCursorTexture, Vector2.zero, cursorMode);
    }
    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(grabCursorTexture, Vector2.zero, cursorMode);

        }
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(handCursorTexture, Vector2.zero, cursorMode);

        }

      

    }
}
