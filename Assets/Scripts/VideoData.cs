using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu]
public class VideoData : ScriptableObject {

    [System.Serializable]
    public class Choices
    {
        [TextArea(3, 20)]
        public string ChoiceText;
        public int ChoiceClips;
    }
    [System.Serializable]
    public class Message
    {
        [TextArea(3, 20)]
        public string MessageText;
        public float timeOnScreen = 3f;
    }
    [System.Serializable]
    public class MessageHolder
    {
        public Message[] messages;
    }
    [System.Serializable]
    public class Video
    {
        public MessageHolder Message;
        public VideoClip mainClip;

        public Choices[] choices;
    }

    public Video[] video;


}
