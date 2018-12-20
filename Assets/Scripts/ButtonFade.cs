using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFade : MonoBehaviour {
    public float buttonAlpha = .65f;
    public Image image;
    public Text text;
    private float alpha = 1;
    public float getAlpha()
    {
        return alpha;
    }
    public void SetAlpha(float a)
    {
        alpha = a;
        if(text!= null && image != null)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(0, buttonAlpha, alpha));
        }
       
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
