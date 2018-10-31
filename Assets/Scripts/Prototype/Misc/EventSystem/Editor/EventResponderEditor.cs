using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Josh.EventSystem
{

    [CustomEditor(typeof(EventResponder))]
    public class EventResponderEditor : Editor
    {
        GUIStyle foldoutStyle;
        float width;
        string newEntryName;
        int lastID = 1;

        SerializedProperty eventTriggersProperty;
        SerializedProperty eventsProperty;
        List<bool> openSections = new List<bool>();

        public void OnEnable()
        {

            foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fixedWidth = 8;
            float ignore;
            foldoutStyle.CalcMinMaxWidth(GUIContent.none, out width, out ignore);

            eventTriggersProperty = serializedObject.FindProperty(EventResponder.EVENT_TRIGGERS_PROPERTY);
            eventsProperty = serializedObject.FindProperty(EventResponder.EVENTS_PROPERTY);
            if (openSections == null || openSections.Count < eventTriggersProperty.arraySize) {
                openSections = new List<bool>(eventTriggersProperty.arraySize);
            }
            CheckForMismatch();
            newEntryName = "Unnamed Event" + eventTriggersProperty.arraySize;
        }

        public override void OnInspectorGUI()
        {
            eventTriggersProperty = serializedObject.FindProperty(EventResponder.EVENT_TRIGGERS_PROPERTY);
            eventsProperty = serializedObject.FindProperty(EventResponder.EVENTS_PROPERTY);
            if (openSections == null || openSections.Count < eventTriggersProperty.arraySize)
            {
                openSections = new List<bool>(eventTriggersProperty.arraySize);
            }

            EditorGUILayout.LabelField("Events", EditorStyles.centeredGreyMiniLabel);
            for (int i = 0; i < eventTriggersProperty.arraySize; i++)
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(eventTriggersProperty.GetArrayElementAtIndex(i),GUIContent.none, GUILayout.ExpandWidth(true));
                if(GUILayout.Button("Trigger", EditorStyles.miniButton))
                {
                    EventResponder.TriggerEvent(eventTriggersProperty.GetArrayElementAtIndex(i).stringValue);
                }
                if(GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(20)))
                {
                    RemoveEntry(i);
                }

                EditorGUILayout.EndHorizontal();

                if (true)
                {
                    EditorGUILayout.PropertyField(eventsProperty.GetArrayElementAtIndex(i), GUIContent.none);
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            newEntryName = EditorGUILayout.TextField(newEntryName);
            if(GUILayout.Button("+", EditorStyles.miniButton))
            {
                if (!CheckIfAlreadyContained())
                {
                    AddNewEntry();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (CheckIfAlreadyContained())
            {
                EditorGUILayout.HelpBox("Already contains new event key!", MessageType.Error);
            }

            CheckForMismatch();

            if (serializedObject.ApplyModifiedProperties())
            {

            }
        }

        private void RemoveEntry(int i)
        {
            eventTriggersProperty.DeleteArrayElementAtIndex(i);
            eventsProperty.DeleteArrayElementAtIndex(i);
            openSections.RemoveAt(i);
        }

        private void AddNewEntry()
        {
            eventTriggersProperty.InsertArrayElementAtIndex(eventTriggersProperty.arraySize);
            eventTriggersProperty.GetArrayElementAtIndex(eventTriggersProperty.arraySize-1).stringValue = newEntryName;
            lastID++;
            newEntryName = "Unnamed Event " + lastID;
            
            eventsProperty.InsertArrayElementAtIndex(eventsProperty.arraySize);
            openSections.Add(false);
        }

        private bool CheckIfAlreadyContained()
        {
            for (int i = 0; i < eventTriggersProperty.arraySize; i++)
            {
                if(eventTriggersProperty.GetArrayElementAtIndex(i).stringValue == newEntryName)
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckForMismatch()
        {
            if(eventTriggersProperty.arraySize != eventsProperty.arraySize)
            {
                int minSize = Mathf.Min(eventsProperty.arraySize, eventTriggersProperty.arraySize);
                for (int i = minSize; i < eventTriggersProperty.arraySize; i++)
                {
                    eventTriggersProperty.DeleteArrayElementAtIndex(i);
                }
                for (int i = minSize; i < eventsProperty.arraySize; i++)
                {
                    eventsProperty.DeleteArrayElementAtIndex(i);
                }
            }
            
        }

    }

}