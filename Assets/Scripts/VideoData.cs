using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
[CreateAssetMenu]
public class VideoData : ScriptableObject {

    [System.Serializable]
    public class Choices
    {
      //  [TextArea(3, 20)]
        public string ChoiceText;

        public int ChoiceClips;
       public string[] options = { "Rigidbody", "Box Collider", "Sphere Collider" };

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
        public string name;
        public VideoClip mainClip;
   
        public float YRot = 0;
        [Space]

        public MessageHolder Message;
      
        [Space]  
        public Choices[] choices;
    }
  //  [ReorderableList]
    public Video[] video;

   
}
