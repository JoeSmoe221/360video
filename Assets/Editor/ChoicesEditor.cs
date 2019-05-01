using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
//using Object = System.Object;

[CustomEditor(typeof(VideoData))]
public class videodataEditor : Editor
{

    public override void OnInspectorGUI()
    {
        VideoData myTarget = (VideoData)target;
        //myTarget.video[0].choices[0].
      //  serializedObject.FindProperty("video").
        base.OnInspectorGUI();


    }
}
[CustomPropertyDrawer(typeof(VideoData.MessageHolder))]
public class MessageHolderEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
       // EditorGUI.PropertyField(position, property.FindPropertyRelative("messages"));

        EditorGUI.BeginProperty(position, label, property);
        label.text = "Messages";
        SerializedProperty list = property.FindPropertyRelative("messages");
        Rect arraylabelRect = new Rect(position.x, position.y, position.width, 16);

        //Rect contentPosition =  EditorGUI.PrefixLabel(position, label);
        EditorGUI.PropertyField(arraylabelRect, list);

        if (list.isExpanded)
        {
            Rect arraySizeRect = new Rect(position.x, position.y + 16, position.width, 16);
            EditorGUI.indentLevel += 1;
            EditorGUI.PropertyField(arraySizeRect, list.FindPropertyRelative("Array.size"));
            //  list.arraySize = EditorGUI.IntField(arraySizeRect,"size" ,list.arraySize);
            EditorGUI.indentLevel -= 1;

            position.y += 16;
            EditorGUI.indentLevel += 1;



            for (int i = 0; i < list.arraySize; i++)
            {
                // string name = "";
                // if (i < (int)Ware.END)
                //   name = ((Ware)i).ToString();


                EditorGUI.PropertyField(position, list.GetArrayElementAtIndex(i));
                position.y += EditorGUI.GetPropertyHeight(list.GetArrayElementAtIndex(i));
            }
            EditorGUI.indentLevel -= 1;
        }
        
        EditorGUI.EndProperty();

        // EditorGUI.IntField(position, 0);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0;
        SerializedProperty list = property.FindPropertyRelative("messages");
        if (list.isExpanded)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                // string name = "";
                // if (i < (int)Ware.END)
                //   name = ((Ware)i).ToString();


                //    EditorGUI.PropertyField(position, list.GetArrayElementAtIndex(i));
                height += EditorGUI.GetPropertyHeight(list.GetArrayElementAtIndex(i));
            }
        }
       

        //   height += property.FindPropertyRelative("ChoiceClips")
        return (16 + height);
    }
}

[CustomPropertyDrawer(typeof(VideoData.Choices))]
public class ChoicesEditor : PropertyDrawer
{
    //float textHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
       // property.FindPropertyRelative
        //    EditorGUI.PrefixLabel(position, label);
        //    EditorGUI.PropertyField(position, property.FindPropertyRelative("ChoiceText"));
        // EditorGUI.PropertyField(position, property.FindPropertyRelative("ChoiceClips"));
        label = EditorGUI.BeginProperty(position, label, property);
        position.y += 16;
        Rect contentPosition = position;//EditorGUI.PrefixLabel(position, label);
        // EditorGUI.indentLevel = 0;
     
        // Custom style
        GUIStyle myStyle = new GUIStyle(EditorStyles.textArea);
        myStyle.fontSize = 11;
        myStyle.wordWrap = true;
        SerializedProperty myTextProperty = property.FindPropertyRelative("ChoiceText");

        GUIContent guiContent = new GUIContent(myTextProperty.stringValue);
       float  textHeight = myStyle.CalcHeight(guiContent, EditorGUIUtility.currentViewWidth) +16;
        Rect textPosition = new Rect(contentPosition.x, contentPosition.y, contentPosition.width, textHeight);
       // textPosition.height = textHeight;
        myTextProperty.stringValue = EditorGUI.TextArea(textPosition, myTextProperty.stringValue, myStyle);

        // Text height


        //   EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("ChoiceText"), GUIContent.none);
        Rect choiceClipRect = new Rect(contentPosition.x, contentPosition.y + textHeight, contentPosition.width, 18f);


        var obj = property.serializedObject.targetObject;
        VideoData myDataClass = obj as VideoData;

        string[] options = new string[myDataClass.video.Length +1];
        options[0] = "main menu";
        for (int i = 1; i < options.Length;i++)
        {
            options[i] = myDataClass.video[i-1].name;
        }
        SerializedProperty myChoiceProperty = property.FindPropertyRelative("ChoiceClips");

        myChoiceProperty.intValue = EditorGUI.Popup(choiceClipRect, "Choice Clip", myChoiceProperty.intValue +1, options) -1;
       // choiceClipRect.y += 16;
      //  EditorGUI.PropertyField(choiceClipRect, property.FindPropertyRelative("ChoiceClips"), GUIContent.none);
        //   position.height += 18f;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        SerializedProperty myTextProperty = property.FindPropertyRelative("ChoiceText");
        GUIStyle myStyle = new GUIStyle(EditorStyles.textArea);
        myStyle.fontSize = 11;
        myStyle.wordWrap = true;

        GUIContent guiContent = new GUIContent(myTextProperty.stringValue);
        float textHeight = myStyle.CalcHeight(guiContent, EditorGUIUtility.currentViewWidth) + 16;
        
        //   height += property.FindPropertyRelative("ChoiceClips")
        return (32+ textHeight +16);
    }
}




[CustomPropertyDrawer(typeof(VideoData.Message))]
public class MessageEditor : PropertyDrawer
{
    //float textHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        // property.FindPropertyRelative
        //    EditorGUI.PrefixLabel(position, label);
        //    EditorGUI.PropertyField(position, property.FindPropertyRelative("ChoiceText"));
        // EditorGUI.PropertyField(position, property.FindPropertyRelative("ChoiceClips"));
        label = EditorGUI.BeginProperty(position, label, property);
        position.y += 16;
        Rect contentPosition = position;//EditorGUI.PrefixLabel(position, label);
                                        // EditorGUI.indentLevel = 0;

        // Custom style
        GUIStyle myStyle = new GUIStyle(EditorStyles.textArea);
        myStyle.fontSize = 11;
        myStyle.wordWrap = true;
        SerializedProperty myTextProperty = property.FindPropertyRelative("MessageText");

        GUIContent guiContent = new GUIContent(myTextProperty.stringValue);
        float textHeight = myStyle.CalcHeight(guiContent, EditorGUIUtility.currentViewWidth) + 16;
        Rect textPosition = new Rect(contentPosition.x, contentPosition.y, contentPosition.width, textHeight);
        // textPosition.height = textHeight;
        myTextProperty.stringValue = EditorGUI.TextArea(textPosition, myTextProperty.stringValue, myStyle);

        // Text height


        //   EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("ChoiceText"), GUIContent.none);
        Rect choiceClipRect = new Rect(contentPosition.x, contentPosition.y + textHeight, contentPosition.width, 18f);


      

        // choiceClipRect.y += 16;
        EditorGUI.PropertyField(choiceClipRect, property.FindPropertyRelative("timeOnScreen"));
        //   position.height += 18f;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        SerializedProperty myTextProperty = property.FindPropertyRelative("MessageText");
        GUIStyle myStyle = new GUIStyle(EditorStyles.textArea);
        myStyle.fontSize = 11;
        myStyle.wordWrap = true;

        GUIContent guiContent = new GUIContent(myTextProperty.stringValue);
        float textHeight = myStyle.CalcHeight(guiContent, EditorGUIUtility.currentViewWidth) + 16;

        //   height += property.FindPropertyRelative("ChoiceClips")
        return (32 + textHeight + 16);
    }
}
