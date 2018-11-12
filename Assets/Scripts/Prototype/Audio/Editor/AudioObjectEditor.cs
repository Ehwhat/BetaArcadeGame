using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioObject))]
public class AudioObjectEditor : Editor {

    private SerializedProperty pitchMin;
    private SerializedProperty pitchMax;

    private void OnEnable()
    {
        pitchMax = serializedObject.FindProperty(AudioObject.PITCH_MAX_PROPERTY);
        pitchMin = serializedObject.FindProperty(AudioObject.PITCH_MIN_PROPERTY);
    }

}
